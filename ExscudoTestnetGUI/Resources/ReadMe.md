# Configuration file

To connect to a peer, you need to set an address

    "peer": "peer.testnet.eontechnology.org:9443",

Seed for the account on behalf of which we will sign transactions

    "seed": "...",

Transaction’s secondary fields, are needed to manage the commission and the transaction lifetime

    "deadline": 60,
    "fee": 10,

# Command description

List of commands available

Command     | Description
:-----------|:--------
refill      | Transfer coins from amount to deposit
withdraw    | Transfer coins from deposit to amount
payment     | Transfer/Send coins to another account
new_account | Create a new network account using its public key
commited    | Output the list of transactions entered the blockchain
uncommited  | Output the list of transactions that did not enter the blockchain
allcommited | Output all lists of transactions entered the blockchain
state       | Account state in the network
attributes  | Attributes of the network of peers maintaining the blockchain 
info        | View EON_id and public key
seed        | Generate a public key for a new account

## Examples

A brief hint with a list of possible commands to use:
```
user@pc:~/EON$ ./eon
Client for EON peer, 2017 v0.11
command available to use:
refill, withdraw, payment, new_account
commited, uncommited, allcommited, state, attributes, info, seed

end...
```

Commands "commited", "uncommitted", "allcommitted", "state" can be executed without specifying an account identifier.

---

### eon state

The current user account from the configuration file will be used by default.
```
user@pc:~/EON$ ./eon state
Client for EON peer, 2017 v0.11
command available to use:
refill, withdraw, payment, new_account
commited, uncommited, allcommited, state, attributes, info, seed

Answer on get state for  EON-QB3PM-Y3MRM-28TFL
Account state: 200 - OK 
Amount:    984.948950
Deposit:     0.000000

end...
```

---

You can check any network account state with its eon_id.
```
user@pc:~/EON$ ./eon state EON-2NTFF-EVEMG-7MBAL
Client for EON peer, 2017 v0.11
command available to use:
refill, withdraw, payment, new_account
commited, uncommited, allcommited, state, attributes, info, seed

Answer on get state for  EON-2NTFF-EVEMG-7MBAL
Account state: 404 - Not Found 
Amount:      0.000000
Deposit:     0.000000

end...
```

---
### eon seed

Public key generation for a new account.
A string of random symbols is created and on its bases EON_id and public key are figured out.
```
user@pc:~/EON$ ./eon seed
Client for EON peer, 2017 v0.11
command available to use:
refill, withdraw, payment, new_account
commited, uncommited, allcommited, state, attributes, info, seed
 
Generate seed for EON account 
     Seed:  fef59c1f61aa10fe9ab8bf0d733d1baf72e6aabf0eabc31511a18aa537733d2b
     Account:  EON-FML9B-ZWC78-7SPRZ
     Public key:  53d99943a81457d3cb5a05261c0a993d74ac8c11ecf780dd7834b85eba7643d2

end...
```

---

### eon info

The command "info" output EON_id and the public key needed to register the network account for the account described in the configuration file.
```
user@pc:~/EON$ ./eon info
Client for EON peer, 2017 v0.11
command available to use:
refill, withdraw, payment, new_account
commited, uncommited, allcommited, state, attributes, info, seed
 
EON account from seed
     Seed:  fef59c1f61aa10fe9ab8bf0d733d1baf72e6aabf0eabc31511a18aa537733d2b
     Account:  EON-FML9B-ZWC78-7SPRZ
     Public key:  53d99943a81457d3cb5a05261c0a993d74ac8c11ecf780dd7834b85eba7643d2
end...
```

---

### eon new_account

After you had registered on the network you can create additional accounts on your behalf.
The public key for the new account is set in the account creation function parameters.
You can get it using the function < eon info > or < eon info new_SEED >.
```
user@pc:~/EON$ ./eon new_account fef59c1f61aa10fe9ab8bf0d733d1baf72e6aabf0eabc31511a18aa537733d2b
Client for EON peer, 2017 v0.11
command available to use:
refill, withdraw, payment, new_account
commited, uncommited, allcommited, state, attributes, info, seed
 
Create new EON account
     Seed:  
     New account:  EON-T4AF6-T3433-EB7GY
     Public key for account:  fef59c1f61aa10fe9ab8bf0d733d1baf72e6aabf0eabc31511a18aa537733d2b

Maker for new account: EON-QB3PM-Y3MRM-28TFL
Timestamp:  1509094502
Network id:  EON-B-2CMBX-669EY-TWFBK

send transaction() <<  "success"

end...
```

---

### eon refill / eon withdraw / eon payment

Use commands refill, withdraw, payment to work with funds in your account.

---

Transfer coins from ammount to depisit
```
user@pc:~/EON$ ./eon refill 100
Client for EON peer, 2017 v0.11
command available to use:
refill, withdraw, payment, new_account
commited, uncommited, allcommited, state, attributes, info, seed

Deposit refill, deposit funds to participate in the generation of blocks
     Amount: 100.000000
send transaction() <<  "success"

end...
```

---

Transfer coins from deposit to ammount
```
user@pc:~/EON$ ./eon withdraw 50
Client for EON peer, 2017 v0.11
command available to use:
refill, withdraw, payment, new_account
commited, uncommited, allcommited, state, attributes, info, seed

Deposit withdraw, stop participating in the generation of blocks
     Amount: 50.000000
send transaction() <<  "success"

end...
``` 

---

Transfer/send coins to another account.
```
user@pc:~/EON$ ./eon payment EON-5VXV4-AFZEZ-7Y4FV 1000
Client for EON peer, 2017 v0.11
command available to use:
refill, withdraw, payment, new_account
commited, uncommited, allcommited, state, attributes, info, seed

Ordinary payment, transfer of coins between two accounts
     to EON account:EON-5VXV4-AFZEZ-7Y4FV
     Amount:1000.000000
Failed transaction with amount: Not enough funds. 

end...
```

---

### eon commited / eon uncommited / eon allcommited

You can view transactions sent and signed by a certain subscriber.
You can check which of them have already entered the blockchain and which ones have not.
If you do not input Eon_id, the command will display information for the account from the configuration file.
```
user@pc:~/EON$ ./eon commited
Client for EON peer, 2017 v0.11
command available to use:
refill, withdraw, payment, new_account
commited, uncommited, allcommited, state, attributes, info, seed

Answer on get commited transactions for EON-QB3PM-Y3MRM-28TFL, page 1 

2017-10-27 12:07:14  Withdraw from deposit              +50.000000 (-0.000010)
2017-10-27 12:03:47  Refill to deposit                 -100.000000 (-0.000010)
2017-10-27 11:55:02  New account EON-T4AF6-T3433-EB7GY             (-0.000010)
2017-10-26 16:03:13  Refill to deposit                 -111.000000 (-0.000010)
2017-10-20 17:51:28  Refill to deposit                  -10.120000 (-0.000010)
2017-10-19 20:19:52  From EON-QPAU2-QV7TD-WT85R          +0.000001
2017-10-19 17:28:25  To   EON-2JXAG-UTWWK-5U7SU          -0.000527 (-0.000010)
2017-10-19 17:18:17  To   EON-2JXAG-UTWWK-5U7SU          -0.000527 (-0.000010)
2017-10-19 17:07:40  From EON-5VXV4-AFZEZ-7Y4FV          +0.123456
2017-10-19 17:04:10  To   EON-5VXV4-AFZEZ-7Y4FV          -0.020000 (-0.000010)
2017-10-19 14:58:46  To   EON-2JXAG-UTWWK-5U7SU          -0.054500 (-0.000010)
2017-10-19 14:58:28  To   EON-2JXAG-UTWWK-5U7SU          -0.054500 (-0.000010)
2017-10-19 14:57:04  To   EON-2JXAG-UTWWK-5U7SU          -0.099000 (-0.000010)
2017-10-19 14:54:48  Withdraw from deposit             +100.150000 (-0.000010)
2017-10-19 14:53:54  Refill to deposit                 -100.150000 (-0.000010)
2017-10-18 13:55:12  Refill to deposit                   -0.000012 (-0.000010)
2017-10-18 13:48:49  Refill to deposit                   -0.100000 (-0.000010)
2017-10-18 12:46:04  To   EON-WN8LK-C7QZH-UJFML          -0.000000 (-0.000010)
2017-10-18 09:40:52  To   EON-QB3PM-Y3MRM-28TFL          -0.000010 (-0.000010)
2017-10-18 09:40:52  From EON-QB3PM-Y3MRM-28TFL          +0.000010
2017-10-10 11:09:11  From EON-2JXAG-UTWWK-5U7SU       +1000.000000

end...
```

---

You can input EON_id after the command "uncommited" ("commited" or "allcommited") to view output.
```
user@pc:~/EON$ ./eon commited  EON-QB3PM-Y3MRM-28TFL
Client for EON peer, 2017 v0.11
command available to use:
refill, withdraw, payment, new_account
commited, uncommited, allcommited, state, attributes, info, seed

Answer on get commited transactions for EON-QB3PM-Y3MRM-28TFL, page 1 

2017-10-27 12:18:03  To   EON-QB3PM-Y3MRM-28TFL        -100.000000 (-0.000010)
2017-10-27 12:18:03  From EON-QB3PM-Y3MRM-28TFL        +100.000000
2017-10-27 12:15:59  To   EON-QB3PM-Y3MRM-28TFL        -100.000000 (-0.000010)
2017-10-27 12:15:59  From EON-QB3PM-Y3MRM-28TFL        +100.000000
2017-10-27 12:14:54  To   EON-QB3PM-Y3MRM-28TFL        -100.000000 (-0.000010)
2017-10-27 12:14:54  From EON-QB3PM-Y3MRM-28TFL        +100.000000
2017-10-27 12:14:30  Withdraw from deposit             +100.000000 (-0.000010)
2017-10-27 12:14:20  Refill to deposit                 -100.000000 (-0.000010)
2017-10-27 12:07:14  Withdraw from deposit              +50.000000 (-0.000010)
2017-10-27 12:03:47  Refill to deposit                 -100.000000 (-0.000010)
2017-10-27 11:55:02  New account EON-T4AF6-T3433-EB7GY             (-0.000010)
2017-10-26 16:03:13  Refill to deposit                 -111.000000 (-0.000010)
2017-10-20 17:51:28  Refill to deposit                  -10.120000 (-0.000010)
2017-10-19 20:19:52  From EON-QPAU2-QV7TD-WT85R          +0.000001
2017-10-19 17:28:25  To   EON-2JXAG-UTWWK-5U7SU          -0.000527 (-0.000010)
2017-10-19 17:18:17  To   EON-2JXAG-UTWWK-5U7SU          -0.000527 (-0.000010)
2017-10-19 17:07:40  From EON-5VXV4-AFZEZ-7Y4FV          +0.123456
2017-10-19 17:04:10  To   EON-5VXV4-AFZEZ-7Y4FV          -0.020000 (-0.000010)
2017-10-19 14:58:46  To   EON-2JXAG-UTWWK-5U7SU          -0.054500 (-0.000010)
2017-10-19 14:58:28  To   EON-2JXAG-UTWWK-5U7SU          -0.054500 (-0.000010)
2017-10-19 14:57:04  To   EON-2JXAG-UTWWK-5U7SU          -0.099000 (-0.000010)
2017-10-19 14:54:48  Withdraw from deposit             +100.150000 (-0.000010)
2017-10-19 14:53:54  Refill to deposit                 -100.150000 (-0.000010)

end...
```

---

You can input page number put number after the command "commited" to view selected page.
```
user@pc:~/EON$ ./eon commited EON-QB3PM-Y3MRM-28TFL 2
Client for EON peer, 2017 v0.11
command available to use:
refill, withdraw, payment, new_account
commited, uncommited, allcommited, state, attributes, info, seed

Answer on get commited transactions page for EON-QB3PM-Y3MRM-28TFL, page 2 

2017-10-19 14:58:28  To   EON-2JXAG-UTWWK-5U7SU          -0.054500 (-0.000010)
2017-10-19 14:57:04  To   EON-2JXAG-UTWWK-5U7SU          -0.099000 (-0.000010)
2017-10-19 14:54:48  Withdraw from deposit             +100.150000 (-0.000010)
2017-10-19 14:53:54  Refill to deposit                 -100.150000 (-0.000010)
2017-10-18 13:55:12  Refill to deposit                   -0.000012 (-0.000010)
2017-10-18 13:48:49  Refill to deposit                   -0.100000 (-0.000010)
2017-10-18 12:46:04  To   EON-WN8LK-C7QZH-UJFML          -0.000000 (-0.000010)
2017-10-18 09:40:52  To   EON-QB3PM-Y3MRM-28TFL          -0.000010 (-0.000010)
2017-10-18 09:40:52  From EON-QB3PM-Y3MRM-28TFL          +0.000010
2017-10-10 11:09:11  From EON-2JXAG-UTWWK-5U7SU       +1000.000000

end...
```

---

You can view all transactions you ever made and which has been included the blockchain. They are outputted in one long list.
```
user@pc:~/EON$ ./eon allcommited
Client for EON peer, 2017 v0.11
command available to use:
refill, withdraw, payment, new_account
commited, uncommited, allcommited, state, attributes, info, seed


2017-10-27 12:15:59  To   EON-QB3PM-Y3MRM-28TFL        -100.000000 (-0.000010)
2017-10-27 12:15:59  From EON-QB3PM-Y3MRM-28TFL        +100.000000
2017-10-27 12:14:54  To   EON-QB3PM-Y3MRM-28TFL        -100.000000 (-0.000010)
2017-10-27 12:14:54  From EON-QB3PM-Y3MRM-28TFL        +100.000000
2017-10-27 12:14:30  Withdraw from deposit             +100.000000 (-0.000010)
2017-10-27 12:14:20  Refill to deposit                 -100.000000 (-0.000010)
2017-10-27 12:07:14  Withdraw from deposit              +50.000000 (-0.000010)
2017-10-27 12:03:47  Refill to deposit                 -100.000000 (-0.000010)
2017-10-27 11:55:02  New account EON-T4AF6-T3433-EB7GY             (-0.000010)
2017-10-26 16:03:13  Refill to deposit                 -111.000000 (-0.000010)
2017-10-20 17:51:28  Refill to deposit                  -10.120000 (-0.000010)
2017-10-19 20:19:52  From EON-QPAU2-QV7TD-WT85R          +0.000001
2017-10-19 17:28:25  To   EON-2JXAG-UTWWK-5U7SU          -0.000527 (-0.000010)
2017-10-19 17:18:17  To   EON-2JXAG-UTWWK-5U7SU          -0.000527 (-0.000010)
2017-10-19 17:07:40  From EON-5VXV4-AFZEZ-7Y4FV          +0.123456
2017-10-19 17:04:10  To   EON-5VXV4-AFZEZ-7Y4FV          -0.020000 (-0.000010)
2017-10-19 14:58:46  To   EON-2JXAG-UTWWK-5U7SU          -0.054500 (-0.000010)
2017-10-19 14:58:28  To   EON-2JXAG-UTWWK-5U7SU          -0.054500 (-0.000010)
2017-10-19 14:57:04  To   EON-2JXAG-UTWWK-5U7SU          -0.099000 (-0.000010)
2017-10-19 14:54:48  Withdraw from deposit             +100.150000 (-0.000010)
2017-10-19 14:53:54  Refill to deposit                 -100.150000 (-0.000010)
2017-10-18 13:55:12  Refill to deposit                   -0.000012 (-0.000010)

2017-10-18 13:48:49  Refill to deposit                   -0.100000 (-0.000010)
2017-10-18 12:46:04  To   EON-WN8LK-C7QZH-UJFML          -0.000000 (-0.000010)
2017-10-18 09:40:52  To   EON-QB3PM-Y3MRM-28TFL          -0.000010 (-0.000010)
2017-10-18 09:40:52  From EON-QB3PM-Y3MRM-28TFL          +0.000010
2017-10-10 11:09:11  From EON-2JXAG-UTWWK-5U7SU       +1000.000000
All page out

end...
```

27.10.2017