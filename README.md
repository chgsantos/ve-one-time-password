# Ve Interactive - C# Developer Logic Assessment

This project has been created by Caio Henrique to be resolution for an assessment created by Ve Interactive.

----------

## The Assessment
The assessment was created based on the instructions below.

### Instructions
Please write a program or API that can generate a one-time password and verify if the password is valid for one user only. 

### The input of the program should be: 
- User ID to generate the password;
- Use the User ID and the password to verify the validity of the password;
- Every generated password should be valid for 30 seconds.

----------

## The Resolution

Some information were not clear to me:

**Should I use a database or not?**
As it is an assessment, I have assumed it was to verify my programming logic, not the technology in use, so I used a list in memory with valid users.

**Could the password be valid for 30 seconds even if it was regenerated within this time?**
I have assumed as yes, as this code could be a 2-step verification api.

**Could two users have the same password?**
Well, I think it could not, as it's written here: "and verify if the password is valid for one user only".

----------

## Usage

It is a MVC application, so you have to run the application and use the following actions.
The return is based on JSend specification, simplified to return only `true` or `false` in `status`.

#### Generates a password for a given User ID:

*http://`url`/Password/GeneratePassword?userId=`userId`*

**Parameters:**

- `userId`: User ID to generate password

**Return:**
```
{
	status: true,		// status of the generation
	data: "000000",		// generated password
	message: null,		// error message
	code: null			// error code
}
```

#### Checks the validity of a given User ID and password:
*http://`url`/Password/ValidatePassword?userId=`userId`&password=`password`*

**Parameters:**

- `userId`: User ID to generate password
- `password`: Password of the given User

**Return:**
```
{
	status: true,		// status of the validity
	data: null,			// nothing
	message: null,		// error message
	code: null			// error code
}
```

----------

## Credits

Developed by Caio Henrique.

Contact info:

- Email: chgsantos@gmail.com 
- Skype: chgsantos
- Mobile: +55 31 99744-1000
