using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3_객체간_통신을_하는_법_Observer.fourth
{
    public interface IObserver
    {

        void personGetNotifiedFromCompany(object sender, int newPersonWage);
        void headhunterGetNotifiedFromCompany(object sender, string Department, string Level, int experiencedWage);
        void personGetNotifiedFromHeadHunter(object sender, string companyName, string Department, string Level, int experiecedWage);
    }
}
