# ExscudoTestnetGUI
Unofficial tool for Exscudo testnet client
V.Sahota Oct 2017

The tool provides a GUI interface to the Windows Exscudo testnet client.  It allow pushbutton access to eon.exe commands
and implements a basic wallet and transactions display.

Wallet view - sync's balances every 5s.  Allows to transfer funds between account and deposit.
Config view - Shows the parameters in config.json , and allows modification.
Transaction view - Allows to view the transaction history (data source from eon commit command)
Debug view - stores the eon.exe output for the the last command (refill/withdraw/send)
Log view - History of eon.exe output

On first start;
1. eon client and openssl will be installed within the program directory.
2.A new SEED will be generated using the recommended openssl method
3.An account will be created and the details reported in debug view. You can use the menu item "Tools>Open Exscudo registration website" and then copy/paste the details. 

Once this is done, the balances in the wallet should soon begin to show correctly rather than report 404.
Menu also has shortcuts to open Explorer in the working folder, and launch command line to eon or openssl.

