namespace API_Movies.Models.DTO
{
    public class ResetPasswordDto
    {
        public Guid VerificationCode {  get; set; }
        public string NewPassword { get; set; }
    }
}
