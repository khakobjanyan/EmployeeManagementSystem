namespace EmployeeManagementSystemAPI.Helpers
{
    public static class EmailBody
    {
        public static string EmailStringBody(string email, string emailToken)
        {

            return $@"<html>
<head>
  <meta charset=""UTF-8"">
  <title>Password Reset</title>
</head>
<body>
  <table width=""100%"" border=""0"" cellspacing=""0"" cellpadding=""0"">
    <tr>
      <td align=""center"" bgcolor=""#f4f4f4"">
        <table width=""600"" border=""0"" cellspacing=""0"" cellpadding=""0"">
          <tr>
            <td align=""center"" bgcolor=""#ffffff"">
              <h1>Password Reset</h1>
              <p>You have requested to reset your password. Click the link below to reset your password:</p>
              <p><a href=""http://localhost:4200/reset?email={email}&code={emailToken}"" target=""_blank"">Reset Password</a></p>
              <p>If you did not request a password reset, please ignore this email.</p>
            </td>
          </tr>
          <tr>
            <td align=""center"" bgcolor=""#333333"">
              <p style=""color: #ffffff; font-size: 14px;"">Employee Management System &copy; 2023</p>
            </td>
          </tr>
        </table>
      </td>
    </tr>
  </table>
</body>
</html>";
        }
    }
}
