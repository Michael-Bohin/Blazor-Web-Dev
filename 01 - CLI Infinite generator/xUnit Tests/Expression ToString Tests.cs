using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

/*      Unit tests of ToString method assert that all leafes of the object tree of Expression 
 *  do return the expected string representation. Therefor foreach leafe of the Expression
 *  desecndant there will be test class that will constuct different variations and than 
 *  check that ToString return expected result. 
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
    public class Expression_ToString_xUnitTests {
        [Fact]
        public void Test1() {

        }
    }

}
