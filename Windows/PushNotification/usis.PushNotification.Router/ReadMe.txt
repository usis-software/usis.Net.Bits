Some configuration issues
=========================

To open service host for a web service:

	netsh http add urlacl url=http://+:80/ user="NT AUTHORITY\NetworkService"
