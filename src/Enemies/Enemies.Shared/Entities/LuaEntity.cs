using System;
using System.Collections.Generic;
using System.Text;
using NLua;

namespace Enemies.Entities
{
    class LuaEntity : Entity
    {
        private Lua _context;

        public LuaEntity(string script)
        {
            _context = new Lua();
            _context.LoadFile(script);
        }
    }
}
