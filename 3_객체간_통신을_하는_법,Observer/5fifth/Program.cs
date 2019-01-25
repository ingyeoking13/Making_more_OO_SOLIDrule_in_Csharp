using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3_객체간_통신을_하는_법_Observer.fifth
{
    class Program
    {
        static void Main(string[] args)
        {
            //initializing
            Company samsung = new Company("samsung");
            Company LG = new Company("LG");
            HeadHunter SaramIn = new HeadHunter("SaramIn");
            HeadHunter Jasoseol = new HeadHunter("Jasoseol");
            HeadHunter JobKorea = new HeadHunter("JobKorea");
            Person yohan = new Person("YoHan");
            Person gaelim = new Person("Gaelim");
            Person kimchi = new Person("Kimchi");
            Person adkb = new Person("adkb");
            Person BlueBird = new Person("BlueBird");

            //register
            samsung.person = new GenericNotifier<int>(
                new IObserver<int>[]
                {
                    yohan.personGetNotifiedFromCompany,
                    gaelim.personGetNotifiedFromCompany,
                    kimchi.personGetNotifiedFromCompany
                }
                );

            samsung.HeadHunter = new GenericNotifier<Tuple<string, string, int>>(
                new IObserver<Tuple<string, string, int>>[]
                {
                    JobKorea.headHunterGetNotifiedFromCompany,
                    Jasoseol.headHunterGetNotifiedFromCompany,
                    SaramIn.headHunterGetNotifiedFromCompany
                }
                );

            SaramIn.person = new GenericNotifier<Tuple<string, string, string, int>>(
                new IObserver<Tuple<string, string, string, int>>[]
                {
                    adkb.personGetNotifiedFromHeadHunter,
                    BlueBird.personGetNotifiedFromHeadHunter
                }
                );

            Jasoseol.headHunter = new GenericNotifier<Tuple<string, string, string, int>>(
                new IObserver<Tuple<string, string, string, int>>[]
                {
                    JobKorea.personGetNotifiedFromHeadHunter
                }
                );

            //notify
            samsung.NotifyNewCrewWanted();
            LG.NotifyNewCrewWanted();

        }
    }
}
