IDoc Router 1.0
===============

Nach der Installation muss die Datei

	IDocRouter.exe.template.config
in
	IDocRouter.exe.config

umbenannt und die Einstellungen der SAP Systeme angepasst werden.
Anschließend kann der Starttyp des Dienstes "IDocRouer" auf "Automatisch" gesetzt werden.

Die eingehenden IDocs werden als Datei im Verzeichnis %PROGRAMDATA%\IDocRouter\Inbound
je sendendes System abgelegt.

Aufgetretene Fehler werden im Windows-Protokoll/Anwendung in der Ereignisanzeige protokolliert.
Der Dienst wird bei Fehler nicht beendet.

Die Debug-Ausgaben können mit DbgView geprüft werden.

Die Dienst läuft als 64-bit Prozess.