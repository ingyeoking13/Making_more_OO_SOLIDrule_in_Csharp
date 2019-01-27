using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3_객체간_통신을_하는_법_Observer.sixth
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
            samsung.person += yohan.personGetNotifiedFromCompanyHandler;
            samsung.person += gaelim.personGetNotifiedFromCompanyHandler;
            samsung.person += kimchi.personGetNotifiedFromCompanyHandler;

            samsung.HeadHunter += JobKorea.headhunterGetNotifiedFromCompanyHandler;
            samsung.HeadHunter += Jasoseol.headhunterGetNotifiedFromCompanyHandler;
            samsung.HeadHunter += SaramIn.headhunterGetNotifiedFromCompanyHandler;

            SaramIn.person += adkb.personGetNotifiedFromHeadHunterHandler;
            SaramIn.person += BlueBird.personGetNotifiedFromHeadHunterHandler;

            Jasoseol.headHunter += JobKorea.personGetNotifiedFromHeadHunterHandler;

            //notify
            samsung.NotifyNewCrewWanted();
            LG.NotifyNewCrewWanted();

        }
    }
}
