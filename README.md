# ExscudoTestnetGUI
Unofficial tool for Exscudo testnet client

<provided as-is for testing , use at your own risk!>

exscudo slack : gassman

The tool provides a GUI interface to the Windows Exscudo testnet client.  It allow pushbutton access to eon.exe commands
and implements a basic wallet and transactions display.

Now includes support for creating and using additional wallets (addresses).

Wallet view -
Sync's balances of all configured accounts every 10s.  Allows to transfer funds between account and deposit. You can select a particular account and send payments or refill/withdraw from the deposit balance.

Config view - Shows the parameters in config.json for the selected account, allows modification but there should be no need to meddle with the defaults.

Transaction view - Allows to view the transaction history both confirmed and pending, for the selected account (data is sourced from eon allcommit command, running in the RAW debug mode)

Debug view - dispalys the eon.exe raw output for the the last command (refill/withdraw/send), also details when it creates accounts etc.

Log view - A longer histroy of eon.exe output


When you start the application the first time it will ask to either create a new account or import and existing primary account via either importing an existing config.json, or by pasting your SEED.  The primary account is the one which you registered against your email.
If you create new accounts, backup the SEED values somewhere so you can recover the accounts if something goes wrong.
You can also export the wallets.json as a backup file, which contains all the info on all the wallets.

If you have created >1 primary account , you can save several wallets.json files and import either when you need to access those accounts.

How it works :
On startup the program reads the current wallets.json and checks whether there is a primary account, if not you can import/paste/new.  The eon.exe , config etc are actually embedded in the application so it does not need to download them, it will deploy eon as it needs.
After the primary account is active, the program will search its commit history to see if this account has created additional accounts, if so these will be added (again you can import a config.json , or paste the seed for each)
The first time you use it, possible your virus checker will remove eon.exe , and you may need to visit the antivirus quarantine and recover this file.
Each account has its own working directory with eon.exe , config.json and readme.md. these are created automatically and checked on startup.

Testing in parallel.
You can leave the app running and use it keep an eye on balances and transactions. In parallel you can go to the CMD line and execute shell commands directly.  Use the menu items to open the working folder or command line for the SELECTED account.