using System;
using System.Collections.Generic;

namespace InfiniteEngine {
    using Q = RationalNumber;

    interface ICombinatoricStructureGenerator<T> {
        public List<T> GetAll();
        public T GetRandomOne();
    }
}