using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3_객체간_통신을_하는_법_Observer.fourth
{
    class Program
    {
        static void Main(string[] args)
        {
            //initializing
            Company samsung = new Company("samsung", 4000);
            Company LG = new Company("LG", 4000);
            HeadHunter SaramIn = new HeadHunter("SaramIn");
            HeadHunter Jasoseol = new HeadHunter("Jasoseol");
            HeadHunter JobKorea = new HeadHunter("JobKorea");

            //register
            samsung.register(new Person("YoHan"));
            samsung.register(new Person("Gaelim"));
            samsung.register(new Person("Kimchi"));
            samsung.register(JobKorea);
            samsung.register(Jasoseol);
            samsung.register(SaramIn);

            SaramIn.register(new Person("adkb"));
            SaramIn.register(new Person("BlueBird"));

            Jasoseol.register(JobKorea);

            //notify
            samsung.NotifyNewCrewWanted();
            LG.NotifyNewCrewWanted();

            SaramIn.notify();
            JobKorea.notify();
            Jasoseol.notify();

        }
    }
}
