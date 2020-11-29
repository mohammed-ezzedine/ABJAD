using ABJAD.Models;
using ABJAD.Models.Exceptions;
using System;
using System.Collections.Generic;
using static ABJAD.Models.Constants;

namespace ABJAD.InterpretEngine
{
    public class Environment : ICloneable
    {
        private Dictionary<string, dynamic> environment;
        private Dictionary<string, dynamic> constants;

        private Dictionary<string, dynamic> local_environment;
        private Dictionary<string, dynamic> local_constants;

        private Environment parent_scope;

        public Environment()
        {
            environment = new Dictionary<string, dynamic>();
            constants = new Dictionary<string, dynamic>();
            local_environment = new Dictionary<string, dynamic>();
            local_constants = new Dictionary<string, dynamic>();
            parent_scope = null;
        }

        private Environment(Environment parent, Dictionary<string, dynamic> env, Dictionary<string, dynamic> consts)
        {
            parent_scope = parent;
            environment = env;
            constants = consts;
            local_environment = new Dictionary<string, dynamic>();
            local_constants = new Dictionary<string, dynamic>();
        }

        public object Clone()
        {
            var consts = new Dictionary<string, dynamic>();
            var env = new Dictionary<string, dynamic>();
            foreach (var key in constants.Keys)
            {
                consts[key] = constants[key];
            }
            foreach (var key in local_constants.Keys)
            {
                consts[key] = local_constants[key];
            }
            foreach (var key in environment.Keys)
            {
                env[key] = environment[key];
            }
            foreach (var key in local_environment.Keys)
            {
                env[key] = local_environment[key];
            }
            return new Environment(this, env, consts);
        }

        public dynamic Get(string key, int line, int index)
        {
            if (local_constants.ContainsKey(key))
                return local_constants[key];
            else if (local_environment.ContainsKey(key))
                return local_environment[key];
            else if (constants.ContainsKey(key))
                return constants[key];
            else if (environment.ContainsKey(key))
                return environment[key];
            else
                throw new AbjadInterpretingException(
                    ErrorMessages.English.UnknownKey(key, line, index),
                    ErrorMessages.Arabic.UnknownKey(key, line, index)
                );
        }

        public dynamic Get(Token token)
        {
            return Get(token.Text, token.Line, token.Index);
        }

        public void Set(string key, dynamic value, int line = 0, int index = 0)
        {
            if (constants.ContainsKey(key) || local_constants.ContainsKey(key))
                throw new AbjadInterpretingException(
                    ErrorMessages.English.ConstantModification(line, index),
                    ErrorMessages.Arabic.ConstantModification(line, index)
                );

            local_environment[key] = value;

            if (environment.ContainsKey(key))
            {
                environment[key] = value;

                if (parent_scope != null)
                {
                    parent_scope.Set(key, value); 
                }
            }
        }

        public void SetConstant(string key, dynamic value, int line, int index)
        {
            if (environment.ContainsKey(key) || local_environment.ContainsKey(key) ||
                constants.ContainsKey(key) || local_constants.ContainsKey(key))
                throw new AbjadInterpretingException(
                    ErrorMessages.English.NameTaken(key, line, index), 
                    ErrorMessages.Arabic.NameTaken(key, line, index));

            local_constants[key] = value;
        }

        public bool ContainsKey(string key)
        {
            return constants.ContainsKey(key)
                || environment.ContainsKey(key)
                || local_constants.ContainsKey(key)
                || local_environment.ContainsKey(key); ;
        } 
    }
}
