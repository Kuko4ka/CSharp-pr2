using System;
using System.IO;
using PolynomialLibrary;
using Xunit;

namespace PolynomialLibrary.Tests
{
    public class PolynomialTests
    {
        #region Конструкторы и свойства

        [Fact]
        public void DefaultConstructor_CreatesZeroPolynomial()
        {
            // Arrange & Act
            var p = new Polynomial();

            // Assert
            Assert.Equal(0, p.Degree);
            Assert.Equal(1, p.Length);
            Assert.Equal(0.0, p[0]);
        }

        [Fact]
        public void DegreeConstructor_ValidDegree_CreatesZeroedArray()
        {
            // Arrange
            int degree = 3;

            // Act
            var p = new Polynomial(degree);

            // Assert
            Assert.Equal(3, p.Degree);
            Assert.Equal(4, p.Length);
            Assert.Equal(0.0, p[3]);
        }

        [Fact]
        public void DegreeConstructor_NegativeDegree_ThrowsArgumentException()
        {
            // Arrange
            int invalidDegree = -1;

            // Act
            Action act = () => new Polynomial(invalidDegree);

            // Assert
            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ParamsConstructor_NullArray_CreatesZeroPolynomial()
        {
            // Arrange
            double[] coeffs = null;

            // Act
            var p = new Polynomial(coeffs);

            // Assert
            Assert.Equal(0, p.Degree);
            Assert.Equal(0.0, p[0]);
        }

        [Fact]
        public void ParamsConstructor_EmptyArray_CreatesZeroPolynomial()
        {
            // Arrange
            double[] coeffs = Array.Empty<double>();

            // Act
            var p = new Polynomial(coeffs);

            // Assert
            Assert.Equal(0, p.Degree);
            Assert.Equal(0.0, p[0]);
        }

        [Fact]
        public void ParamsConstructor_WithLeadingZeros_TrimsPolynomial()
        {
            // Arrange
            double[] coeffs = { 1, 2, 0, 0 };

            // Act
            var p = new Polynomial(coeffs);

            // Assert
            Assert.Equal(1, p.Degree);
            Assert.Equal(2, p.Length);
        }

        [Fact]
        public void ParamsConstructor_AllZeros_TrimsToSingleZero()
        {
            // Arrange
            double[] coeffs = { 0, 0, 0 };

            // Act
            var p = new Polynomial(coeffs);

            // Assert
            Assert.Equal(0, p.Degree);
            Assert.Equal(1, p.Length);
            Assert.Equal(0.0, p[0]);
        }

        [Fact]
        public void CopyConstructor_ValidPolynomial_CreatesDeepCopy()
        {
            // Arrange
            var original = new Polynomial(1, 2, 3);

            // Act
            var copy = new Polynomial(original);
            copy[0] = 99;

            // Assert
            Assert.Equal(1, original[0]);
            Assert.Equal(99, copy[0]);
        }

        [Fact]
        public void CopyConstructor_Null_ThrowsArgumentNullException()
        {
            // Arrange
            Polynomial other = null;

            // Act
            Action act = () => new Polynomial(other);

            // Assert
            Assert.Throws<ArgumentNullException>(act);
        }

        #endregion

        #region Индексаторы

        [Fact]
        public void IntIndexer_Get_ReturnsCoefficient()
        {
            // Arrange
            var p = new Polynomial(1, 2, 3);

            // Act
            double coeff = p[2];

            // Assert
            Assert.Equal(3.0, coeff);
        }

        [Fact]
        public void IntIndexer_SetNonLeading_UpdatesCoefficient()
        {
            // Arrange
            var p = new Polynomial(1, 2, 3);

            // Act
            p[1] = -7;

            // Assert
            Assert.Equal(-7.0, p[1]);
            Assert.Equal(2, p.Degree);
        }

        [Fact]
        public void IntIndexer_SetLeadingToZero_TrimsPolynomial()
        {
            // Arrange
            var p = new Polynomial(1, 2, 3);

            // Act
            p[2] = 0;

            // Assert
            Assert.Equal(1, p.Degree);
            Assert.Equal(2, p.Length);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(3)]
        public void IntIndexer_GetOutOfRange_ThrowsIndexOutOfRangeException(int badIndex)
        {
            // Arrange
            var p = new Polynomial(1, 2);

            // Act
            Action act = () => _ = p[badIndex];

            // Assert
            Assert.Throws<IndexOutOfRangeException>(act);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(3)]
        public void IntIndexer_SetOutOfRange_ThrowsIndexOutOfRangeException(int badIndex)
        {
            // Arrange
            var p = new Polynomial(1, 2);

            // Act
            Action act = () => p[badIndex] = 5;

            // Assert
            Assert.Throws<IndexOutOfRangeException>(act);
        }

        [Theory]
        [InlineData(2.0, 9.0)]
        [InlineData(0.0, 1.0)]
        [InlineData(-1.0, 6.0)]
        public void DoubleIndexer_EvaluatesPolynomialAtX(double x, double expected)
        {
            // Arrange
            var p = new Polynomial(1, -2, 3); // 3x^2 - 2x + 1

            // Act
            double actual = p[x];

            // Assert
            Assert.Equal(expected, actual, 10);
        }

        #endregion

        #region ToString и Input

        [Theory]
        [InlineData(new double[] { 0 }, "0")]
        [InlineData(new double[] { 5 }, "5")]
        [InlineData(new double[] { -5 }, "-5")]
        [InlineData(new double[] { 1, -1, 1 }, "x^2 - x + 1")]
        [InlineData(new double[] { 0, 2, 0, -1 }, "-x^3 + 2x")]
        [InlineData(new double[] { -1, 1, -2 }, "-2x^2 + x - 1")]
        public void ToString_ReturnsExpectedFormattedString(double[] coeffs, string expected)
        {
            // Arrange
            var p = new Polynomial(coeffs);

            // Act
            string actual = p.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Input_ValidConsoleInput_PopulatesPolynomial()
        {
            // Arrange
            var origIn = Console.In;
            var origOut = Console.Out;
            var input = new StringReader($"2{Environment.NewLine}1{Environment.NewLine}2{Environment.NewLine}3{Environment.NewLine}");
            var p = new Polynomial();
            var expected = new Polynomial(1, 2, 3);

            try
            {
                Console.SetIn(input);
                Console.SetOut(new StringWriter());

                // Act
                p.Input();
            }
            finally
            {
                Console.SetIn(origIn);
                Console.SetOut(origOut);
            }

            // Assert
            Assert.Equal(expected, p);
        }

        [Theory]
        [InlineData("-1")]
        [InlineData("abc")]
        public void Input_InvalidDegree_ThrowsFormatException(string badDegree)
        {
            // Arrange
            var origIn = Console.In;
            var origOut = Console.Out;
            var p = new Polynomial();

            try
            {
                Console.SetIn(new StringReader(badDegree + Environment.NewLine));
                Console.SetOut(new StringWriter());

                // Act
                Action act = () => p.Input();

                // Assert
                Assert.Throws<FormatException>(act);
            }
            finally
            {
                Console.SetIn(origIn);
                Console.SetOut(origOut);
            }
        }

        [Fact]
        public void Input_InvalidCoefficient_ThrowsFormatException()
        {
            // Arrange
            var origIn = Console.In;
            var origOut = Console.Out;
            var p = new Polynomial();

            try
            {
                Console.SetIn(new StringReader($"1{Environment.NewLine}not_a_number{Environment.NewLine}"));
                Console.SetOut(new StringWriter());

                // Act
                Action act = () => p.Input();

                // Assert
                Assert.Throws<FormatException>(act);
            }
            finally
            {
                Console.SetIn(origIn);
                Console.SetOut(origOut);
            }
        }

        #endregion

        #region Арифметические операции

        [Fact]
        public void Addition_LongerPlusShorterPolynomial_ReturnsSum()
        {
            // Arrange
            var p1 = new Polynomial(1, 2, 3);
            var p2 = new Polynomial(4, 5);
            var expected = new Polynomial(5, 7, 3);

            // Act
            Polynomial actual = p1 + p2;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Addition_ShorterPlusLongerPolynomial_ReturnsSum()
        {
            // Arrange
            var p1 = new Polynomial(4, 5);
            var p2 = new Polynomial(1, 2, 3);
            var expected = new Polynomial(5, 7, 3);

            // Act
            Polynomial actual = p1 + p2;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Subtraction_EqualPolynomials_ReturnsZeroPolynomial()
        {
            // Arrange
            var p1 = new Polynomial(1, 2, 3);
            var p2 = new Polynomial(1, 2, 3);
            var expected = new Polynomial(0);

            // Act
            Polynomial actual = p1 - p2;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Subtraction_LongerMinusShorter_ReturnsDifference()
        {
            // Arrange
            var p1 = new Polynomial(1, 2, 3);
            var p2 = new Polynomial(5, 1);
            var expected = new Polynomial(-4, 1, 3);

            // Act
            Polynomial actual = p1 - p2;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Subtraction_ShorterMinusLonger_ReturnsDifference()
        {
            // Arrange
            var p1 = new Polynomial(5, 1);
            var p2 = new Polynomial(1, 2, 3);
            var expected = new Polynomial(4, -1, -3);

            // Act
            Polynomial actual = p1 - p2;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Addition_PolynomialPlusNumber_ReturnsSum()
        {
            // Arrange
            var p = new Polynomial(1, 2);
            double number = 5.0;
            var expected = new Polynomial(6, 2);

            // Act
            Polynomial actual = p + number;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Addition_NumberPlusPolynomial_ReturnsSum()
        {
            // Arrange
            var p = new Polynomial(1, 2);
            double number = 5.0;
            var expected = new Polynomial(6, 2);

            // Act
            Polynomial actual = number + p;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Multiplication_PolynomialByNumber_ReturnsScaledPolynomial()
        {
            // Arrange
            var p = new Polynomial(1, -2, 3);
            double number = 2.0;
            var expected = new Polynomial(2, -4, 6);

            // Act
            Polynomial actual = p * number;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Multiplication_NumberByPolynomial_ReturnsScaledPolynomial()
        {
            // Arrange
            var p = new Polynomial(1, -2, 3);
            double number = 2.0;
            var expected = new Polynomial(2, -4, 6);

            // Act
            Polynomial actual = number * p;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Multiplication_PolynomialByZero_ReturnsZeroPolynomial()
        {
            // Arrange
            var p = new Polynomial(1, -2, 3);
            var expected = new Polynomial(0);

            // Act
            Polynomial actual = p * 0.0;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Multiplication_TwoPolynomials_ReturnsProduct()
        {
            // Arrange
            var p1 = new Polynomial(1, 1);           // x + 1
            var p2 = new Polynomial(-1, 1);          // x - 1
            var expected = new Polynomial(-1, 0, 1); // x^2 - 1

            // Act
            Polynomial actual = p1 * p2;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Multiplication_ByZeroPolynomial_ReturnsZeroPolynomial()
        {
            // Arrange
            var p1 = new Polynomial(1, 2, 3);
            var pZero = new Polynomial();
            var expected = new Polynomial(0);

            // Act
            Polynomial actual = p1 * pZero;

            // Assert
            Assert.Equal(expected, actual);
        }

        #endregion

        #region Операции сравнения, Equals и GetHashCode

        [Fact]
        public void Equality_SameReference_ReturnsTrue()
        {
            // Arrange
            var p = new Polynomial(1, 2);

            // Act
            bool actual = p == p;

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void Equality_EqualValues_ReturnsTrue()
        {
            // Arrange
            var p1 = new Polynomial(1, 2);
            var p2 = new Polynomial(1, 2);

            // Act
            bool actual = p1 == p2;

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void Equality_DifferentCoefficients_ReturnsFalse()
        {
            // Arrange
            var p1 = new Polynomial(1, 2);
            var p2 = new Polynomial(1, 3);

            // Act
            bool actual = p1 == p2;

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void Equality_DifferentLengths_ReturnsFalse()
        {
            // Arrange
            var p1 = new Polynomial(1, 2);
            var p2 = new Polynomial(1, 2, 3);

            // Act
            bool actual = p1 == p2;

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void Equality_LeftNull_ReturnsFalse()
        {
            // Arrange
            Polynomial pNull = null;
            var p = new Polynomial(1, 2);

            // Act
            bool actual = pNull == p;

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void Equality_RightNull_ReturnsFalse()
        {
            // Arrange
            var p = new Polynomial(1, 2);
            Polynomial pNull = null;

            // Act
            bool actual = p == pNull;

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void Inequality_DifferentPolynomials_ReturnsTrue()
        {
            // Arrange
            var p1 = new Polynomial(1, 2);
            var p2 = new Polynomial(1, 3);

            // Act
            bool actual = p1 != p2;

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void Inequality_EqualPolynomials_ReturnsFalse()
        {
            // Arrange
            var p1 = new Polynomial(1, 2);
            var p2 = new Polynomial(1, 2);

            // Act
            bool actual = p1 != p2;

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void GreaterThan_HigherDegree_ReturnsTrue()
        {
            // Arrange
            var deg1 = new Polynomial(10, 1);
            var deg2 = new Polynomial(1, 0, 1);

            // Act
            bool actual = deg2 > deg1;

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void GreaterThan_SameDegreeHigherCoeff_ReturnsTrue()
        {
            // Arrange
            var smaller = new Polynomial(1, 0, 1);
            var bigger = new Polynomial(1, 0, 2);

            // Act
            bool actual = bigger > smaller;

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void GreaterThan_EqualPolynomials_ReturnsFalse()
        {
            // Arrange
            var p1 = new Polynomial(1, 0, 1);
            var p2 = new Polynomial(1, 0, 1);

            // Act
            bool actual = p1 > p2;

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void LessThan_LowerDegree_ReturnsTrue()
        {
            // Arrange
            var deg1 = new Polynomial(10, 1);
            var deg2 = new Polynomial(1, 0, 1);

            // Act
            bool actual = deg1 < deg2;

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void LessThan_SameDegreeLowerCoeff_ReturnsTrue()
        {
            // Arrange
            var smaller = new Polynomial(1, 0, 1);
            var bigger = new Polynomial(1, 0, 2);

            // Act
            bool actual = smaller < bigger;

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void LessThan_EqualPolynomials_ReturnsFalse()
        {
            // Arrange
            var p1 = new Polynomial(1, 0, 1);
            var p2 = new Polynomial(1, 0, 1);

            // Act
            bool actual = p1 < p2;

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void GreaterOrEqual_EqualOrGreater_ReturnsTrue()
        {
            // Arrange
            var p1 = new Polynomial(1, 0, 2);
            var p2 = new Polynomial(1, 0, 1);
            var p3 = new Polynomial(1, 0, 2);

            // Act
            bool greater = p1 >= p2;
            bool equal = p1 >= p3;

            // Assert
            Assert.True(greater);
            Assert.True(equal);
        }

        [Fact]
        public void LessOrEqual_EqualOrLess_ReturnsTrue()
        {
            // Arrange
            var p1 = new Polynomial(1, 0, 1);
            var p2 = new Polynomial(1, 0, 2);
            var p3 = new Polynomial(1, 0, 1);

            // Act
            bool less = p1 <= p2;
            bool equal = p1 <= p3;

            // Assert
            Assert.True(less);
            Assert.True(equal);
        }

        [Fact]
        public void Equals_EqualPolynomial_ReturnsTrue()
        {
            // Arrange
            var p1 = new Polynomial(1, 2, 3);
            object p2 = new Polynomial(1, 2, 3);

            // Act
            bool actual = p1.Equals(p2);

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void Equals_DifferentType_ReturnsFalse()
        {
            // Arrange
            var p = new Polynomial(1, 2, 3);
            object other = "not a polynomial";

            // Act
            bool actual = p.Equals(other);

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void Equals_Null_ReturnsFalse()
        {
            // Arrange
            var p = new Polynomial(1, 2, 3);

            // Act
            bool actual = p.Equals(null);

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void GetHashCode_EqualPolynomials_ReturnSameHash()
        {
            // Arrange
            var p1 = new Polynomial(1, 2, 3);
            var p2 = new Polynomial(1, 2, 3);

            // Act
            int hash1 = p1.GetHashCode();
            int hash2 = p2.GetHashCode();

            // Assert
            Assert.Equal(hash1, hash2);
        }

        #endregion

        #region true/false и приведение типов

        [Fact]
        public void TrueOperator_NonZeroPolynomial_EvaluatesToTrue()
        {
            // Arrange
            var p = new Polynomial(0, 1);

            // Act
            bool actual = p ? true : false;

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void FalseOperator_ZeroPolynomial_EvaluatesToFalse()
        {
            // Arrange
            var p = new Polynomial();

            // Act
            bool actual = p ? true : false;

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void ImplicitConversion_DoubleToPolynomial_CreatesConstantPolynomial()
        {
            // Arrange
            double value = 4.5;

            // Act
            Polynomial actual = value;

            // Assert
            Assert.Equal(0, actual.Degree);
            Assert.Equal(4.5, actual[0]);
        }

        [Fact]
        public void ExplicitConversion_PolynomialToDouble_ReturnsFreeTerm()
        {
            // Arrange
            var p = new Polynomial(7.2, 3.0);

            // Act
            double actual = (double)p;

            // Assert
            Assert.Equal(7.2, actual);
        }

        #endregion
    }
}