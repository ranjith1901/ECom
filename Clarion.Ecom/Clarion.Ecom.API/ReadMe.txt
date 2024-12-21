Add Package Microsoft.EntityFrameworkCore
Add Package Microsoft.EntityFrameworkCore.SqlServer
Add Package Microsoft.EntityFrameworkCore.Design
Add Package Microsoft.EntityFrameworkCore.Tools
Add Package Microsoft.AspNetCore.Authentication.Jwt
Add Package System.IdentityModel.Tokens.Jwt
Add Microsoft.AspNetCore.Authentication.JwtBearer
Add Microsoft.AspNetCore.Authorization
Add Serilog.AspNetCore


Scaffold-DbContext -Connection Name=ClarionECOMCS Microsoft.EntityFrameworkCore.SqlServer -Context "ClarionECOMDBContext" -outputdir Models -Force -UseDatabaseNames
