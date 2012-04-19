gooddata-csharp
===============
There are two basic classes
* SSOProvider.cs
* ApiWrapper.cs

SSOProvider
===============
The SSOProvider has two dependencies:
* gpg4win [http://www.gpg4win.org/]
* starksoftopenpg library, [http://sourceforge.net/projects/starksoftopenpg/].
    The ddl included in this project is a modified version of 1.0.40.
	My modification fixes the Sign method to respect the armor attribute which is required for integration with gooddata

To get the code running you'll need to:
* Download the gooddata public key so you can encrypt the user data, http://developer.gooddata.com/docs/gooddata-sso.pub
* Install gpg4win
* Open a cmd prompt
* Run: "gpg --version" to verify the installation.
* Run: "gpg --import C:\[path]\gooddata-sso.pub" to import the gooddata public key. NOTE: Replace [path]
* Run: "gpg --gen-key" and follow the command prompts. Use something like gooddata@yourcompanydomain.com as the email address
* DONT FORGET YOUR PASSPHRASE
* Run: "gpg --output C:\[path]\gooddata_yourcompany.pub --export gooddata@yourcompanydomain.com"
* Run: "gpg --output C:\[path]\gooddata_yourcompany_private.key --export-secret-key gooddata@yourcompanydomain.com"
* Run: "gpg --list-keys" This should return two entries: 1) security@gooddata.com 2) gooddata@yourcompanydomain.com  

To run on a different machine you'll need to import your public and private keys with the following commands
* Run: "gpg --import C:\gooddata_yourcompany.pub" to import your company public key.
* Run: "gpg --allow-secret-key-import --import C:\gooddata_yourcompany_private.key" to import your company public key.

To run the tests you'll need to set the recipient, email and passphrase to get the tests to work.


Reference instructions:
* http://quantumlab.net/pine_privacy_guard/howto_setup_gpg.html
* http://developer.gooddata.com/docs/sso
    

ApiWrapper
---------------
Currently only contains methods for:
* Authenticate
* User
* Project


Sample GPG Commands
---------------
#Sign
gpg --armor -u gooddata@gmail.com --output jon.kind@gmail.com_userdata.txt_signed.txt --sign jon.kind@gmail.com_userdata.txt

#Encrypt
gpg --armor --output jon.kind@gmail.com_userdata.txt_signed.txt --encrypt --recipient security@gooddata.com jon.kind@gmail.com_userdata.txt_signed.txt

#Sign & Encrypt
gpg --armor --output jon.kind@gmail.com_userdata._encryptedsigned.txt --recipient security@gooddata.com --encrypt --sign jon.kind@gmail.com_userdata.txt

#Decrypt
gpg --output jon.kind@gmail.com._decrupt.txt --decrypt jon.kind@gmail.com_userdata._encryptedsigned.txt