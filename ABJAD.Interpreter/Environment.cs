using ABJAD.Models.Exceptions;
using System;
using System.Collections.Generic;

namespace ABJAD.Interpreter
{
    public class Environment : ICloneable
    {
        private Dictionary<string, dynamic> environment;
        private Dictionary<string, dynamic> constants;

        public Environment(Dictionary<string, dynamic> environment, Dictionary<string, dynamic> constants)
        {
            this.environment = environment;
            this.constants = constants;
        }

        public Environment()
        {
            environment = new Dictionary<string, dynamic>();
            constants = new Dictionary<string, dynamic>();
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public dynamic Get(string key)
        {
            if (constants.ContainsKey(key))
                return constants[key];
            else if (environment.ContainsKey(key))
                return environment[key];
            else
                throw new AbjadInterpretingException($"Key {key} was not found in the scope.");
        }

        public void Set(string key, dynamic value)
        {
            if (constants.ContainsKey(key))
                throw new AbjadInterpretingException("Cannot change the value of a constant.");

            environment[key] = value;
        }

        public void SetConstant(string key, dynamic value)
        {
            if (environment.ContainsKey(key))
                throw new AbjadInterpretingException($"Variable with name {key} already exists.");

            if (constants.ContainsKey(key))
                throw new AbjadInterpretingException($"Constant with name {key} already exists.");

            constants[key] = value;
        }

        public bool ContainsKey(string key)
        {
            return constants.ContainsKey(key) ^ environment.ContainsKey(key);
        }
    }
}
