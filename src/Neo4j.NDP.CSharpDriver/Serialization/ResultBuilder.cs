using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace Neo4j.NDP.CSharpDriver.Serialization
{
    /// <summary>
    /// Builds results of type <typeparamref name="T" /> from records items in the form of <see cref="IMessageList"/>
    /// </summary>
    public class ResultBuilder<T> : IResultBuilder<T>
    {
        private bool hasBeenInitialized = false;
        private Func<object[], object> resultFactory = null;
        private Func<IMessageObject, object>[] casters = null;

        /// <summary>
        /// Builds results of type <typeparamref name="T" /> from records items in the form of <see cref="IMessageList"/>
        /// </summary>
        /// <param name="recordItems">Record items to build the result of.</param>
        /// <typeparam name="T">
        /// The type to construct through its constructor with the values
        /// of <paramref name="recordItems"/> as arguments to the constructor.
        /// </typeparam>
        public T Build(IMessageList recordItems)
        {
            if (recordItems == null) throw new ArgumentNullException("recordItems");

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

            casters = new Func<IMessageObject, object>[length];

            for (int i = 0; i < length; i++)
            {
                Func<IMessageObject, object> caster = MatchTypes(arguments[i], recordItems.Items[i]);

                if (caster == null) throw new InvalidOperationException(string.Format("Argument of type {0} does not match record item with type {1}", arguments[i].FullName, recordItems.Items[i].Type));
            
                casters[i] = caster;
            }
        }


        private Func<IMessageObject, object> MatchTypes(Type type, IMessageObject messageObject)
        {
            if (type == typeof(bool) && messageObject is IMessageBool) 
            {
                return mo => MessageObjectConversionExtensions.ToBool(mo);
            }
            else if (type == typeof(double) && messageObject is IMessageDouble) 
            {
                return mo => MessageObjectConversionExtensions.ToDouble(mo);
            }
            else if (type == typeof(Int64) && messageObject is IMessageInt) 
            {
                return mo => MessageObjectConversionExtensions.ToInt(mo);
            }
            else if (type == typeof(int) && messageObject is IMessageInt) 
            {
                return mo => (int)MessageObjectConversionExtensions.ToInt(mo);
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
            else if (type == typeof(IRelationship) && messageObject is IMessageStructure && 
                (messageObject as IMessageStructure).Signature == StructureSignature.Relationship)
            {
                return mo => MessageObjectConversionExtensions.ToRelationship(mo);
            }
            else 
            {
                return null;
            }
        }


        private Type[] GetArgumentsAndResultFactory()
        {
            if (typeof(T) == typeof(bool))
            {
                resultFactory = parms => (bool)parms[0];
                return new Type[] { typeof(bool) };
            }
            else if (typeof(T) == typeof(double))
            {
                resultFactory = parms => (double)parms[0];
                return new Type[] { typeof(double) };
            }
            else if (typeof(T) == typeof(int))
            {
                resultFactory = parms => (int)parms[0];
                return new Type[] { typeof(int) };
            }
            else if (typeof(T) == typeof(Int64))
            {
                resultFactory = parms => (Int64)parms[0];
                return new Type[] { typeof(Int64) };
            }
            else if (typeof(T) == typeof(string))
            {
                resultFactory = parms => (string)parms[0];
                return new Type[] { typeof(string) };
            }
            else if (typeof(T) == typeof(INode))
            {
                resultFactory = parms => (INode)parms[0];
                return new Type[] { typeof(INode) };
            }
            else if (typeof(T) == typeof(IRelationship))
            {
                resultFactory = parms => (IRelationship)parms[0];
                return new Type[] { typeof(IRelationship) };
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
    }
}

