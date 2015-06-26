using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace Neo4j.NDP.CSharpDriver.Serialization
{
    public class ResultBuilder<T>
    {
        private bool hasBeenInitialized = false;
        private Func<object[], object> resultFactory = null;
        private Func<IMessageObject, object>[] casters = null;

 
        public T Build(IMessageList recordItems)
        {
            if (!hasBeenInitialized)
            {
                Initialize(recordItems);
            }

            if (casters.Length != recordItems.Items.Count) throw new InvalidOperationException("Unexpected number of record items received");

            object[] parms = new object[recordItems.Items.Count];
            for (int i = 0; i < parms.Length; i++)
            {
                parms[i] = casters[i](recordItems.Items[i]);
            }

            return (T)resultFactory(parms);
        }

        private void Initialize(IMessageList recordItems)
        {
            Type[] arguments = GetArgumentsAndResultFactory();

            ValidateArgumentsAndBuildCastTable(arguments, recordItems);

            hasBeenInitialized = true;
        }


        private void ValidateArgumentsAndBuildCastTable(Type[] arguments, IMessageList recordItems)
        {
            if (arguments.Length != recordItems.Items.Count) throw new InvalidOperationException(string.Format("Returned record item count does not match the type {0} constructor item count", typeof(T).FullName));

            int length = arguments.Length;

            Func<IMessageObject, object>[] localCasters = new Func<IMessageObject, object>[length];

            for (int i = 0; i < length; i++)
            {
                Func<IMessageObject, object> caster = MatchTypes(arguments[i], recordItems.Items[i]);

                if (caster == null) throw new InvalidOperationException(string.Format("Argument of type {0} does not match record item with type {1}", arguments[i].FullName, recordItems.Items[i].Type));
            
                localCasters[i] = caster;
            }

            this.casters = localCasters; // Avoid half results
        }


        private Func<IMessageObject, object> MatchTypes(Type type, IMessageObject messageObject)
        {
            if (type == typeof(bool) && messageObject is IMessageBool) 
            {
                return mo => MessageObjectConversionExtensions.ToBool(mo);
            }
            else if (type == typeof(string) && messageObject is IMessageText) 
            {
                return mo => MessageObjectConversionExtensions.ToString(mo);
            }
            else if (type == typeof(INode) && messageObject is IMessageStructure && 
                     (messageObject as IMessageStructure).Signature == StructureSignature.Node)
            {
                return mo => MessageObjectConversionExtensions.ToNode(mo);
            }
            else 
            {
                return null;
            }
        }


        private Type[] GetArgumentsAndResultFactory()
        {
            if (IsTuple(typeof(T)))
            {
                resultFactory = parms => Activator.CreateInstance(typeof(T), parms);
                return typeof(T).GetGenericArguments();
            }
            else 
            {
                IEnumerable<ConstructorInfo> constructors = typeof(T).
                    GetConstructors(BindingFlags.Public | BindingFlags.Instance).
                    Where(f => f.GetParameters().Length > 0);

                if (constructors.Count() == 0) throw new InvalidOperationException(string.Format("The type {0} does not have any constructors taking 1 or more arugments", typeof(T).FullName));
                if (constructors.Count() > 1) throw new InvalidOperationException(string.Format("The type {0} has more than 1 constructors taking 1 or more arugments", typeof(T).FullName));

                ConstructorInfo constructor = constructors.Single();

                resultFactory = parms => constructor.Invoke(parms);
                return constructor.GetParameters().Select(f => f.ParameterType).ToArray();
            }
        }

        private static bool IsTuple(Type type)
        {
            if (!type.IsGenericType) return false;

            Type genericType = type.GetGenericTypeDefinition();
            if (type == typeof(Tuple<,>)) return true;
            if (type == typeof(Tuple<,,>)) return true;
            if (type == typeof(Tuple<,,,>)) return true;
            if (type == typeof(Tuple<,,,,>)) return true;
            if (type == typeof(Tuple<,,,,,>)) return true;
            if (type == typeof(Tuple<,,,,,,>)) return true;
            if (type == typeof(Tuple<,,,,,,,>)) return true;

            return false;
        }
    }
}

