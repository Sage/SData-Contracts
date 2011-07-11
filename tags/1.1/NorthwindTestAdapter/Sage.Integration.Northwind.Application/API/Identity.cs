using System;
using System.Collections.Generic;
using System.Text;

namespace Sage.Integration.Northwind.Application.API
{
    [Serializable]
    public class Identity : IComparable, IEquatable<Identity>
    {
        public static int GetId(Identity id)
        {
            int result;

            if (Int32.TryParse(id.Id, out result))
                return result;

            return 0;
        }

        public static int GetId(string id)
        {
            int result;

            if (Int32.TryParse(id, out result))
                return result;

            return 0;
        }
        #region Fields
        
        private string _id;
        private string _entityName;

        #endregion

        #region Ctor.

        public Identity()
            : this(string.Empty, string.Empty)
        {
        }

        public Identity(string entityName, string id)
        {

            this._entityName = entityName;
            this._id = id;
        }

        #endregion

        #region Properties

        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        //public string EntityName
        //{
        //    get { return entityName; }
        //    set { entityName = value; }
        //}

        #endregion

        #region override Object members

        public override string ToString()
        {
            return /*this.entityName + "." + */this._id;
        }

        public override bool Equals(object obj)
        {
            Identity identity = obj as Identity;
            if (identity == null)
                return false;

            return this.Equals(identity);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        //public override int GetHashCode()
        //{
        //    int hash = 23;
        //    hash = hash * 37 + _id.GetHashCode();
        //    hash = hash * 37 + _entityName.GetHashCode();
        //    return hash; 
        //}

        #endregion

        #region IComparable Members

        /// <summary>
        /// Compares the current instance with another object of the same type.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>A 32-bit signed integer that indicates the relative order of the objects
        /// being compared. The return value has these meanings: Value Meaning Less than
        /// zero This instance is less than obj. Zero This instance is equal to obj.
        /// Greater than zero This instance is greater than obj.</returns>
        /// <exception cref="System.ArgumentException">obj is not the same type as this instance.</exception>
        public int CompareTo(object obj)
        {
            Identity identity;

            identity = obj as Identity;
            if (identity == null)
                throw new ArgumentException("Objects are not of the same type.");


            if (!this._entityName.Equals(identity._entityName))
                throw new ArgumentException("Identities are not of the Entity.");

            return this._id.CompareTo(identity._id);
        }

        #endregion

        #region IEquatable<Identity> Members

        public bool Equals(Identity identity)
        {
            if (!_entityName.Equals(identity._entityName))
                return false;

            return this._id.Equals(identity._id);
        }

        #endregion
    }
}
