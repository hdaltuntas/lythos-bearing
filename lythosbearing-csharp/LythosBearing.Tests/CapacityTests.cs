using System;
using Xunit;
using LythosBearing;

namespace LythosBearing.Tests
{
    public class CapacityTests
    {
        [Fact]
        public void EffectiveArea_Strip_LoadCentered()
        {
            var result = Capacity.EffectiveArea("strip", 2.0, 0, 0, 0);
            Assert.Equal(2.0, result.B);
            Assert.Equal(1.0, result.L);
            Assert.Equal(2.0, result.Area);
        }

        [Fact]
        public void EffectiveArea_Rectangle_EccentricLoad()
        {
            var result = Capacity.EffectiveArea("rectangle", 2.0, 3.0, 0.2, 0.3);
            // beff = 2 - 2*0.2 = 1.6
            // leff = 3 - 2*0.3 = 2.4
            // min = 1.6, max = 2.4
            Assert.Equal(1.6, result.B);
            Assert.Equal(2.4, result.L);
            Assert.Equal(3.84, result.Area, 5);
        }

        [Fact]
        public void GeneralEquation_Undrained_Skempton()
        {
            var factors = new Factors.BearingFactors { Nc = 5.14, Nq = 1, Ngamma = 0 };
            var shape = new Factors.ShapeFactors { sc = 1.2, sq = 1, sgamma = 1 };

            var result = Capacity.GeneralEquation("skempton", 50, 10, 20, 2.0, factors, shape, undrained: true);
            // qc = 50 * 5.14 * 1.2 * 1 = 308.4
            // qq = 10
            // qg = 0
            Assert.Equal(318.4, result.qUlt, 5);
        }
    }
}
