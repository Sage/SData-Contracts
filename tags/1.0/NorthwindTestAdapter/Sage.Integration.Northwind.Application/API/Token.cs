using System;
using System.Collections.Generic;
using System.Text;

namespace Sage.Integration.Northwind.Application.API
{

    [Serializable]
    public class Token : IComparable
    {
        public static int GetId(Token token)
        {
            int result;

            if (Int32.TryParse(token.Id.Id, out result))
                return result;

            return 0;
        }

        /// <summary>
        /// Serializes an instance of type <see cref="Token"/> to a string.
        /// </summary>
        /// <remarks>
        /// The string representation is as followed:
        /// id + SequenceNo + InitRequest
        /// id: 29 chars
        /// SequenceNo: 10 chars
        /// InitRequest: 1 char (1=true; 0=false)
        /// Each part is right padded with ' '.
        /// </remarks>
        /// <param name="token">The object to be serialized.</param>
        /// <returns>A string representing the serialized token.</returns>
        public static string SerializeTokenToString(Token token)
        {
            string strId = token.Id.Id.PadRight(29, ' ');
            string strSequenceNo = token.SequenceNumber.ToString().PadRight(10, ' '); ;
            string strInitRequest = (token.InitRequest) ? "1" : "0";

            // concat and return.
            return strId + strSequenceNo + strInitRequest;
        }

        /// <summary>
        /// Creates a new instance of type <see cref="Token"/> from a token's string representation.
        /// </summary>
        /// <param name="strToken">The string representation of a token.</param>
        /// <returns>The new created token.</returns>
        public static Token DeserializeToken(string strToken)
        {
            Token resultToken = new Token();
            Identity identity = new Identity();

            identity = new Identity(string.Empty, strToken.Substring(0, 29).TrimEnd(' '));
            resultToken.Id = identity;
            resultToken.SequenceNumber = Int32.Parse(strToken.Substring(29, 10).TrimEnd(' '));
            resultToken.InitRequest = (strToken.Substring(39, 1) == "1") ? true : false;

            return resultToken;
        }

        #region Fields

        private int sequenceNumber;
        private Identity id;
        private bool initRequest;

        #endregion

        #region Ctor.

        public Token(Identity id, int sequenceNumber)
        {
            this.SequenceNumber = sequenceNumber;
            this.id = id;
            initRequest = false;
        }

        public Token(Identity id, int sequenceNumber, bool initRequest)
        {
            this.SequenceNumber = sequenceNumber;
            this.id = id;
            this.initRequest = initRequest;
        }

        public Token()
        {
            this.SequenceNumber = 0;
            this.id = new Identity();
            initRequest = false;
        }

        #endregion

        #region Properties

        public int SequenceNumber
        {
            get { return sequenceNumber; }
            set { sequenceNumber = value; }
        }

        public Identity Id
        {
            get { return id; }
            set { id = value; }
        }

        public bool InitRequest
        {
            get { return initRequest; }
            set { initRequest = value; }
        }

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
            Token token;

            token = obj as Token;
            if (token == null)
                throw new ArgumentException("Objects are not of the same type.");
            if (this.sequenceNumber < token.sequenceNumber)
                return -1;
            if (this.sequenceNumber > token.sequenceNumber)
                return 1;

            return this.id.CompareTo(token.id);
        }

        #endregion
    }
}
