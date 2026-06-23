using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities.Common;

namespace TTERP.Domain.Entities
{
    public class TaskAssignment : BaseAuditEntity
    {
        public int TaskId { get; set; }
        public Task? Task { get; set; }
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }
        public int Role { get; set; } // responsible, observer, reviewer...

        // Created
        //  ↓
        //Assigned
        //  ↓
        //In Progress
        //  ↓
        //Blocked(opsiyonel)
        //  ↓
        //Completed
        //  ↓
        //Approved(ileride)


        //Order → Task
        //Örnek:

        //Order geldi
        //otomatik task oluşur:
        //“Ürünü hazırla”
        //“Faturala”
        //“Sevkiyat yap”

        //👉 Bu sana ileride workflow motoru açar.
    }
}
