namespace DAL
{
	public class Employee
	{
		private int id;
		private string email;
		public Employee(int id,string email)
		{
			this.id = id;
			this.email = email;
		}
		public int Id { get { return id; } }
		public string Email { get { return email; } }

		public override string ToString()
		{
			return $"{id.ToString()}-{email}";
		}
	}
}