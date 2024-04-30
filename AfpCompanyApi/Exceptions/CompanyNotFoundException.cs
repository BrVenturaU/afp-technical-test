namespace AfpCompanyApi.Exceptions
{
    public class CompanyNotFoundException: NotFoundException
    {
        public CompanyNotFoundException(int id)
            :base($"The company with id: {id} was not found.")
        {
            
        }
    }
}
