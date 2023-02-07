using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThisIsCSharp
{
    class StructArray<T> where T : struct   //값 형식으로 T 제한
    {
        public T[] Array { get; set; }
        public StructArray(int size) => Array = new T[size];    //생성자
    }

    class RefArray<T> where T : class       //참조 형식으로 T 제한
    {
        public T[] Array { get; set; }
        public RefArray(int size) => Array = new T[size];       //생성자
    }

    class Base { }
    class Derived : Base { }
    class BaseArray<U> where U : Base   //U는 Base의 파생클래스로 제한
    {
        public U[] Array { get; set; }
        public BaseArray(int size) => Array = new U[size];     //생성자

        public void CopyArray<T>(T[] Source) where T : U
        {
            Source.CopyTo(Array, 0);
        }
    }

    class ConstraintsOnTypeParameters
    {
        public static T CreateInstance<T>() where T : new() => new T(); //매개변수 없는 생성자를 가진 객체로 T 제한(ex : Base, Derived 클래스)

        static void Main(string[] args)
        {
            StructArray<int> a = new StructArray<int>(3);
            a.Array[0] = 0;
            a.Array[1] = 1;
            a.Array[2] = 2;

            RefArray<StructArray<double>> b = new RefArray<StructArray<double>>(3);
            b.Array[0] = new StructArray<double>(5);
            //b.Array[0].Array[0] = 3.2;
            b.Array[1] = new StructArray<double>(10);
            b.Array[2] = new StructArray<double>(1005);

            BaseArray<Base> c = new BaseArray<Base>(3);
            c.Array[0] = new Base();
            c.Array[1] = new Derived();
            c.Array[2] = CreateInstance<Base>();

            BaseArray<Derived> d = new BaseArray<Derived>(3);
            d.Array[0] = new Derived();
            d.Array[1] = CreateInstance<Derived>();
            d.Array[2] = CreateInstance<Derived>();

            BaseArray<Derived> e = new BaseArray<Derived>(3);
            e.CopyArray<Derived>(d.Array);
        }
    }
}
