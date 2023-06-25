using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVA.UI.Helpers.Base
{
    public class EntityBaseComposite : IObjectState
    {
        private ObjectState _objectState;
        public int ID { get; set; }
        public string Code { get; set; }

        public ObjectState ObjectState
        {
            get
            {
                if (ID == default(int)) { this._objectState = ObjectState.Insert; } // ID = 0
                return this._objectState;
            }
            set { this._objectState = value; }
        }
    }
}
