namespace Luxena.Travel.Domain
{

    [RU("Подразделение", "Подразделения")]
    public partial class Department : Party
    {

        public override PartyType Type => PartyType.Department;

        public virtual Organization Organization { get; set; }

        public override string ToString()
        {
            if (Organization == null)
                return Name;

            return string.Format(DomainRes.Department_Format_ToString, Name, Organization);
        }


        public new class Service : Service<Department>
        {

        }

    }


    public partial class DepartmentListDetailDto : PartyListDetailDto
    {
    }

    public partial class DepartmentListDetailContractService :
        PartyListDetailContractService<Department, Department.Service, DepartmentListDetailDto>
    {
    }

}