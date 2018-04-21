Create self signed certificate
==============================

from:	https://www.codeproject.com/Articles/18601/An-easy-way-to-use-certificates-for-WCF-security
and:	https://blogs.msdn.microsoft.com/james_osbornes_blog/2010/12/10/selfhosting-a-wcf-service-over-https/

Create a self signed root certificate
-------------------------------------

makecert.exe -sk RootCA -sky signature -pe -n CN=<machineName> -r -sr LocalMachine -ss Root MyCA.cer

-sk		key container name
-sr		certificate store location
-ss		certificate store name

makecert -r -pe -n "CN=usis-svr07" -b 01/01/2017 -e 01/01/2020 -sky exchange Server.cer -sv Server.pvk
makecert.exe -sk server -sky exchange -pe -n CN=<machineName> -ir LocalMachine -is Root -ic MyCA.cer -sr LocalMachine -ss My <certificate path>
makecert.exe -sk server -sky exchange -pe -n CN=usis-svr07 -ir LocalMachine -is Root -ic usis-svr07_CA.cer -sr LocalMachine -ss My usis-svr07_Test.cer


-r		create self signed certificate
-pe		mark private key as exportable
-n		certificate subject name
-b		start of the validity period; default to now.
-e		end of validity period; defaults to 2039
-sky	subject key type (exchange)
-sv		private key file
-ir		Issuer's certificate store location
-ic		Issuer's certificate file

pvk2pfx.exe -pvk Server.pvk -spc Server.cer -pfx Server.pfx    

netsh http add urlacl url=https://+:443/Test user=EVERYONE

netsh http add sslcert ipport=0.0.0.0:2323 certhash=8780328191a9b981da94f47f541dc7d4e60aef57 appid={7bb35e7a-84aa-4031-9c03-257f3c3b4ed2}