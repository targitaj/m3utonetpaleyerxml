using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TILDE.Db;
using TILDE.Db.DTO;

namespace TILDE.Services
{
    public class AccountService
    {
        public IList<User> Users
        {
            get
            {
                using (var context = new TILDEDataContext())
                {
                    return context.Users.ToList();
                }
            }
        }

        public UserWithTotal GetLargestBorrowerByUserId(int id)
        {
            using (var context = new TILDEDataContext())
            {
                UserWithTotal res = null;

                var user = context.Users.First(f => f.Id == id);

                //User borrowers it is place where he is creditor(money giver)
                var borrowerAmSumm = (from b in user.Сreditors
                                      group b by b.Borrower
                                      into groupB
                                      select new { Borrower = groupB.Key, TotAm = groupB.Sum(am => am.Amount) });

                if (borrowerAmSumm.Count() != 0)
                {
                    var maxAm = borrowerAmSumm.Max(bm => bm.TotAm);
                    var maxBor = borrowerAmSumm.First(f => f.TotAm == maxAm);

                    res = new UserWithTotal()
                    {
                        User = maxBor.Borrower,
                        Total = maxBor.TotAm
                    };
                }

                return res;
            }
        }

        public UserWithTotal GetLargestCreditorByUserId(int id)
        {
            using (var context = new TILDEDataContext())
            {
                UserWithTotal res = null;

                var user = context.Users.First(f => f.Id == id);

                //User creditors it is place where he is borrower(money taker)
                var borrowerAmSumm = (from b in user.Borrowers
                                      group b by b.Сreditor
                                          into groupB
                                          select new { Сreditor = groupB.Key, TotAm = groupB.Sum(am => am.Amount) });

                if (borrowerAmSumm.Count() != 0)
                {
                    var maxAm = borrowerAmSumm.Max(bm => bm.TotAm);
                    var maxBor = borrowerAmSumm.First(f => f.TotAm == maxAm);

                    res = new UserWithTotal()
                    {
                        User = maxBor.Сreditor,
                        Total = maxBor.TotAm
                    };
                }

                return res;
            }
        }

        public decimal GetAvarageBorrowingByUserId(int id)
        {
            using (var context = new TILDEDataContext())
            {
                decimal res = 0;
                var user = context.Users.First(f => f.Id == id);

                if (user.Borrowers.Count != 0)
                {
                    res = (decimal)user.Borrowers.Sum(b => b.Amount) / (decimal)user.Borrowers.Count;
                }

                return res;
            }
        }
    }
}
