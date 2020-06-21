# Projekt Zespołowy Grupa 1

nazwa projektu: projekt bankomat

	warstwa baza danych:
		(transakcje)
		customers
		credit cards
		bank accounts
		
	warstwa dostepu do danych: data access layer:
		app.config:
			5 metod
			select/update/
			connection open
			connection close

	warstwa:
		3 klasy:
			customer
			bank account
			credit card
	
	warstwa formularz:
		atm:
			
		bank(opcjonalnie):
			

	wytwarzanie oprogramowania:
		model kaskadowy:
			analiza -> projektowanie -> implementacja(developement) -> testowanie
		model cykliczny:
	
				analizowanie

		testowanie			projektowanie

				implementacja

	4 diagramy:
		diagram use case:
			system:
				bankomat
			2 aktorow:
				klient
				bank
			polecenia:
				autoryzuj sie		extend
									<----	wyplac pieniadze
									<----	sprawdz stan konta
									<----	wyswietl historie
									<----	wykonaj przelew

	diagram activity: ogolnie
	diagram sequence: 1 sekwencja albo olac
	diagram klas !najwazniejszy!:
		ten co jest w pdf
		+ bank account
		+ credit card
		
	lista osob i jakie maja role:
	1.project manager
	2.architekt
	3.analityk biznesowy
	4.Lead dev
	5.developer
	6.junior dev
	7.Lead QA
	8.tester
	9.grafik
	
	co projekt ma robic: ma symulowac dzialanie bankomatu
	to jest diagram use case
	to jest diagram activity
	to jest diagram sequence
	
	dodać 2 tabele
	
	zrobic print screeny tabele, kod, uml, lista kto co robi
