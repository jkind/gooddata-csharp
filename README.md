#gooddata-csharp

There are two basic classes

1. SSOProvider.cs
2. ApiWrapper.cs

##SSOProvider
####The SSOProvider has two dependencies:
1. gpg4win http://www.gpg4win.org/
2. starksoftopenpg library, http://sourceforge.net/projects/starksoftopenpg/.
    The ddl included in this project is a modified version of 1.0.40.
	My modification fixes the Sign method to respect the armor attribute which is required for integration with gooddata

####To get the code running you'll need to:
1. Install gpg4win http://www.gpg4win.org/download.html
2. Setup your keys

To setup keys (*Note: replace 'yourcompany.com' and [path] with approproate values*)

1. Open cmd prompt
2. Run: "gpg --version" to verify the installation.
3. Run: "gpg --import [path]\gooddata-sso.pub"  to import the gooddata public key. **NOTE:** Replace [path]. Keys are located in this project or can be found here, http://developer.gooddata.com/docs/gooddata-sso.pub
4. Run: "gpg --import [path]\gooddata-sso-testing.pub"  to import the gooddata testing public key.

To generate a new public and private key

1. Run: "gpg --gen-key" and follow the command prompts. Use something like gooddata@yourcompany.com as the email address. **DONT FORGET YOUR PASSPHRASE**
2. Run: "gpg --output C:\[path]\gooddata_yourcompany.pub --export gooddata@yourcompany.com"
3. Run: "gpg --output C:\[path]\gooddata_yourcompany_private.key --export-secret-key gooddata@yourcompany.com"

Using either a preexisting key or the key you just create

1. Run: "gpg --import C:\gooddata_yourcompany.pub" to import your company's public key.
2. Run: "gpg --allow-secret-key-import --import C:\gooddata_yourcompany_private.key" to import your company's public key.

Verify key setup

1. Run: "gpg --list-keys" This should return 3 entries: 1) security@gooddata.com 2) ops@gooddata.com 3) gooddata@yourcompany.com
2. Run: "gpg --armor -u gooddata@yourcompany.com --output test_userdata_signed.txt --sign [path]\test_userdata.txt" *Note: A sample file can be found in this project*
3. Run: "gpg --armor --output test_userdata_encrypted.txt --encrypt --recipient security@gooddata.com test_userdata.txt"
3. Run: "gpg --output test_userdata_decrypted.txt --decrypt test_userdata_encrypted.txt"



####To run the tests:
You'll need to set the recipient, email and passphrase in the wen.config to get the tests to work.

Reference instructions:

http://quantumlab.net/pine_privacy_guard/howto_setup_gpg.html

http://developer.gooddata.com/docs/sso
    

###Sample GPG Commands

Sign: gpg --armor -u gooddata@yourcompany.com --output test_userdata_signed.txt --sign test_userdata.txt

Encrypt: gpg --armor --output test_userdata_encrypted.txt --encrypt --recipient security@gooddata.com test_userdata.txt

Sign &amp; Encrypt: gpg --armor --output test_userdata_encryptedsigned.txt --recipient security@gooddata.com --encrypt --sign test_userdata.txt

Decrypt: gpg --output test_userdata_decrypted.txt --decrypt test_userdata_encrypted.txt

##ApiWrapper
Currently only contains methods for:

1. Authenticate
2. User
3. Project
