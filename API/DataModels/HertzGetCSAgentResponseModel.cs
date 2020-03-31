namespace Hertz.API.DataModels
{
    public class HertzGetCSAgentResponseModel
    {
        public string AgentUserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long ? RoleId { get; set; }
        public long ? AgentNumber { get; set; }
        public string EmailAddress { get; set; }
    }
}
