using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotonAPI.API
{
    public class Authorization
    {
        public Dictionary<string, string> AuthParameters = new Dictionary<string, string>();

        public void AddAuthParameter(string name, string value) => AuthParameters.Add(name, value);

        public void RemoveAuthParameter(string key) => AuthParameters.Remove(key);

        public void ClearAuthParameters() => AuthParameters.Clear();
    }
}
