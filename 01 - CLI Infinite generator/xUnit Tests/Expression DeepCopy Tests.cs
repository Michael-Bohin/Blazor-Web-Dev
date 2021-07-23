using System;
using Xunit;

/*  DeepCopies are great shelters for hidden bugs. 
 *  Foreach leafe class of object tree
 *  Construct identical expected and tested expression. (different instances obviously)
 *  Let tested expression create deepcopy. 
 *  Test that: 
 *  1. Deepcopy and tested expression return same string.
 *  2. After modificationA of tested, both return expected string. 
 *      (tested returns modifiedA, deepcopy stays unchanged)
 *  3. After modificationB of deepcopy, both return expected string. 
 *      (tested returns modifiedA, deepcopy return modifiedB)
 *      
 *      Currently tested leafe classes are: 
 *      1. Addition 
 *      2. Multiplication 
 *      3. Subtraction 
 *      4. Division
 *      5. Minus (Unary expr)
 *      6. Integer
 *      7. RealNumber 
 *      8. Fraction 
 *      9. Variable  
 */

namespace xUnitTests {
    public class Expression_DeepCopy_xUnitTests {
        [Fact]
        public void Test1() {

        }
    }
}
