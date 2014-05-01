using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA.Db;

namespace DPA.Services
{
    public class AccountService
    {
        public IList<LegalStatus> LegalStatuses
        {
            get
            {
                using (var context = new DPADataContext())
                {
                    return context.LegalStatus.ToList();
                }
            }
        }

        public void AddPerson(Person person)
        {
            using (var context = new DPADataContext())
            {
                context.Persons.InsertOnSubmit(person);
                context.SubmitChanges();
            }
        }

        public IList<Department> Departments
        {
            get
            {
                using (var context = new DPADataContext())
                {
                    return context.Departments.ToList();
                }
            }
        }

        public void AddDepartment(Department department)
        {
            using (var context = new DPADataContext())
            {
                context.Departments.InsertOnSubmit(department);
                context.SubmitChanges();
            }
        }

        public IList<Person> SearchPerson(string personName, string personalCodeNmr)
        {
            using (var context = new DPADataContext())
            {
                return context.Persons.Where(
                    p =>
                        (string.IsNullOrEmpty(personName) || p.PersonName.Contains(personName)) &&
                        (string.IsNullOrEmpty(personalCodeNmr) || p.PersonalCodeNmr.Contains(personalCodeNmr))).ToList();
            }
        }
    }
}
