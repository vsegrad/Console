<#
    .SYNOPSIS
        Enables the specified Sitecore user.

    .DESCRIPTION
        The Enable-User command gets a user and enables the account in Sitecore.

        The Identity parameter specifies the Sitecore user to get. You can specify a user by its local name or fully qualified name.
        You can also specify user object variable, such as $<user>.

    .PARAMETER Identity
        Specifies the Sitecore user by providing one of the following values.

            Local Name
                Example: michael
            Fully Qualified Name
                Example: sitecore\michael

    .PARAMETER Instance
        Specifies the Sitecore user by providing an instance of a user.

    .INPUTS
        System.String
        Represents the identity of one or more users.

        Sitecore.Security.Accounts.User
        One or more user instances.
    
    .OUTPUTS
        None.

    .NOTES
        Michael West

    .LINK
        http://michaellwest.blogspot.com

    .EXAMPLE
        PS master:\> Enable-User -Identity michael

    .EXAMPLE
        PS master:\> Get-User -Filter * | Enable-User

#>