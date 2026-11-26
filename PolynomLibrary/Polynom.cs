using System;
using System.Text;

namespace PolynomialLibrary
{
    public class Polynomial
    {
        private const double Epsilon = 1e-12;
        private double[] _coeffs; 

        
        public int Degree => _coeffs.Length - 1;

        
        public int Length => _coeffs.Length;

        #region Конструктори

       
        public Polynomial()
        {
            _coeffs = new double[] { 0.0 };
        }

        
        public Polynomial(int degree)
        {
            if (degree < 0)
                throw new ArgumentException("Ступінь полінома не може бути від'ємним.");

            _coeffs = new double[degree + 1];
        }

       
        public Polynomial(params double[] coeffs)
        {
            if (coeffs == null || coeffs.Length == 0)
            {
                _coeffs = new double[] { 0.0 };
            }
            else
            {
                _coeffs = new double[coeffs.Length];
                for (int i = 0; i < coeffs.Length; i++)
                {
                    _coeffs[i] = coeffs[i];
                }
                Trim();
            }
        }

        
        public Polynomial(Polynomial other)
        {
            if (other is null)
                throw new ArgumentNullException(nameof(other));

            _coeffs = new double[other._coeffs.Length];
            for (int i = 0; i < other._coeffs.Length; i++)
            {
                _coeffs[i] = other._coeffs[i];
            }
        }

        #endregion

        
        private void Trim()
        {
            int newLength = _coeffs.Length;
            while (newLength > 1 && Math.Abs(_coeffs[newLength - 1]) < Epsilon)
            {
                newLength--;
            }

            if (newLength != _coeffs.Length)
            {
                double[] trimmed = new double[newLength];
                for (int i = 0; i < newLength; i++)
                {
                    trimmed[i] = _coeffs[i];
                }
                _coeffs = trimmed;
            }
        }

        #region Індексатори

        
        public double this[int index]
        {
            get
            {
                if (index < 0 || index >= _coeffs.Length)
                    throw new IndexOutOfRangeException("Індекс виходить за межі полінома.");
                return _coeffs[index];
            }
            set
            {
                if (index < 0 || index >= _coeffs.Length)
                    throw new IndexOutOfRangeException("Індекс виходить за межі полінома.");
                _coeffs[index] = value;
                Trim();
            }
        }

        
        public double this[double x]
        {
            get
            {
                double result = 0.0;
                for (int i = _coeffs.Length - 1; i >= 0; i--)
                {
                    result = result * x + _coeffs[i];
                }
                return result;
            }
        }

        #endregion

        #region Введення та ToString

        
        public void Input()
        {
            Console.Write("Введіть ступінь полінома: ");
            if (!int.TryParse(Console.ReadLine(), out int degree) || degree < 0)
                throw new FormatException("Некоректний ступінь полінома.");

            _coeffs = new double[degree + 1];
            for (int i = 0; i <= degree; i++)
            {
                Console.Write($"Коефіцієнт при x^{i}: ");
                if (!double.TryParse(Console.ReadLine(), out double val))
                    throw new FormatException("Некоректне числове значення.");
                _coeffs[i] = val;
            }
            Trim();
        }

        public override string ToString()
        {
            if (Degree == 0 && Math.Abs(_coeffs[0]) < Epsilon)
                return "0";

            StringBuilder sb = new StringBuilder();
            bool isFirst = true;

            for (int i = _coeffs.Length - 1; i >= 0; i--)
            {
                double c = _coeffs[i];
                if (Math.Abs(c) < Epsilon)
                    continue;

                if (!isFirst)
                {
                    sb.Append(c > 0 ? " + " : " - ");
                }
                else if (c < 0)
                {
                    sb.Append("-");
                }

                double absC = Math.Abs(c);
                if (i == 0 || Math.Abs(absC - 1.0) >= Epsilon)
                {
                    sb.Append(absC.ToString("G"));
                }

                if (i == 1)
                {
                    sb.Append("x");
                }
                else if (i > 1)
                {
                    sb.Append($"x^{i}");
                }

                isFirst = false;
            }

            return sb.ToString();
        }

        #endregion

        #region Арифметичні операції

        
        public static Polynomial operator +(Polynomial a, Polynomial b)
        {
            int maxLen = a.Length > b.Length ? a.Length : b.Length;
            double[] result = new double[maxLen];

            for (int i = 0; i < maxLen; i++)
            {
                double valA = i < a.Length ? a._coeffs[i] : 0.0;
                double valB = i < b.Length ? b._coeffs[i] : 0.0;
                result[i] = valA + valB;
            }

            return new Polynomial(result);
        }

        public static Polynomial operator -(Polynomial a, Polynomial b)
        {
            int maxLen = a.Length > b.Length ? a.Length : b.Length;
            double[] result = new double[maxLen];

            for (int i = 0; i < maxLen; i++)
            {
                double valA = i < a.Length ? a._coeffs[i] : 0.0;
                double valB = i < b.Length ? b._coeffs[i] : 0.0;
                result[i] = valA - valB;
            }

            return new Polynomial(result);
        }

        
        public static Polynomial operator +(Polynomial p, double number)
        {
            Polynomial res = new Polynomial(p);
            res._coeffs[0] += number;
            res.Trim();
            return res;
        }

        public static Polynomial operator +(double number, Polynomial p)
        {
            return p + number;
        }

        
        public static Polynomial operator *(Polynomial p, double number)
        {
            double[] result = new double[p.Length];
            for (int i = 0; i < p.Length; i++)
            {
                result[i] = p._coeffs[i] * number;
            }
            return new Polynomial(result);
        }

        public static Polynomial operator *(double number, Polynomial p)
        {
            return p * number;
        }

        
        public static Polynomial operator *(Polynomial a, Polynomial b)
        {
            double[] result = new double[a.Degree + b.Degree + 1];

            for (int i = 0; i < a.Length; i++)
            {
                for (int j = 0; j < b.Length; j++)
                {
                    result[i + j] += a._coeffs[i] * b._coeffs[j];
                }
            }

            return new Polynomial(result);
        }

        #endregion

        #region Операції порівняння

        public static bool operator ==(Polynomial a, Polynomial b)
        {
            if (ReferenceEquals(a, b)) return true;
            if (a is null || b is null) return false;
            if (a.Length != b.Length) return false;

            for (int i = 0; i < a.Length; i++)
            {
                if (Math.Abs(a._coeffs[i] - b._coeffs[i]) >= Epsilon)
                    return false;
            }
            return true;
        }

        public static bool operator !=(Polynomial a, Polynomial b)
        {
            return !(a == b);
        }

        public static bool operator >(Polynomial a, Polynomial b)
        {
            if (a.Degree != b.Degree)
                return a.Degree > b.Degree;

            for (int i = a.Degree; i >= 0; i--)
            {
                if (Math.Abs(a._coeffs[i] - b._coeffs[i]) >= Epsilon)
                    return a._coeffs[i] > b._coeffs[i];
            }
            return false;
        }

        public static bool operator <(Polynomial a, Polynomial b)
        {
            if (a.Degree != b.Degree)
                return a.Degree < b.Degree;

            for (int i = a.Degree; i >= 0; i--)
            {
                if (Math.Abs(a._coeffs[i] - b._coeffs[i]) >= Epsilon)
                    return a._coeffs[i] < b._coeffs[i];
            }
            return false;
        }

        public static bool operator >=(Polynomial a, Polynomial b) => !(a < b);
        public static bool operator <=(Polynomial a, Polynomial b) => !(a > b);

        public override bool Equals(object obj)
        {
            return obj is Polynomial other && this == other;
        }

        public override int GetHashCode()
        {
            int hash = 17;
            for (int i = 0; i < _coeffs.Length; i++)
            {
                hash = hash * 31 + _coeffs[i].GetHashCode();
            }
            return hash;
        }

        #endregion

        #region Операції true/false та приведення типів

        
        public static bool operator true(Polynomial p)
        {
            return !(p.Degree == 0 && Math.Abs(p._coeffs[0]) < Epsilon);
        }

        public static bool operator false(Polynomial p)
        {
            return p.Degree == 0 && Math.Abs(p._coeffs[0]) < Epsilon;
        }

       
        public static implicit operator Polynomial(double value)
        {
            return new Polynomial(new double[] { value });
        }

        
        public static explicit operator double(Polynomial p)
        {
            return p._coeffs[0];
        }

        #endregion
    }
}