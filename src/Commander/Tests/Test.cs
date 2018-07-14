using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Commander;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class Test
    {
        [Test]
        public void Test1()
        {
            var charCommander = new CharCommander();
            
            foreach (var v in "hello world")
            {
                charCommander.Execute(v);
            }

            var v1 = charCommander.Undo();
            var v2 = charCommander.Undo();
            var v3 = charCommander.Undo();
            var v4 = charCommander.Undo();
            var v5 = charCommander.Redo();
            var v6 = charCommander.Undo();
            var v7 = charCommander.Execute('z');
            var v8 = charCommander.Redo();


            Assert.AreEqual('d', v1);
            Assert.AreEqual('l', v2);
            Assert.AreEqual('r', v3);
            Assert.AreEqual('o', v4);
            Assert.AreEqual('o', v5);
            Assert.AreEqual('o', v6);
            Assert.AreEqual('z', v7);
            Assert.AreEqual('z', v8);

            for (var i = 0; i < 10; i++)
            {
                charCommander.Undo();
            }

            for (var i = 0; i < 10; i++)
            {
                charCommander.Redo();
            }
        }

        
    }
}
