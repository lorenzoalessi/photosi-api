### Autore - Lorenzo Alessi

# PHOTOSI API

Api gateway della prova pratica di Photosi.

Espone delle API REST per effettuare delle operazioni contattando i microservizi.

## PREREQUISITI

1. **.Net 8**
2. **PostgreSQL 16**
3. **IDE per l'avvio del progetto (Rider / Visual Studio)**

## AVVIO DEL PROGETTO

Il progetto non è dotato di database e parte di default sulla porta 1000
(modificabile dal file launchSettings.json)

I file di properties appsettings e appsettings.Development.json presentano una sezione
**"Settings"** con i seguenti parametri:

| Proprietà              | Tipo   | Default value                              | Descrizione                       |
|------------------------|--------|--------------------------------------------|-----------------------------------|
| PhotosiProductsUrl     | string | http://localhost:1001/api/v1/products      | Url API microserizio Products     |
| PhotosiUsersUrl        | string | http://localhost:1002/api/v1/users         | Url API microserizio AddressBooks |
| PhotosiOrdersUrl       | string | http://localhost:1003/api/v1/orders        | Url API microserizio Orders       |
| PhotosiAddressBooksUrl | string | http://localhost:1004/api/v1/address-books | Url API microserizio Users        |

## OPERAZIONE EFFETTUABILI

### Creazione di un utente

end-point -> POST: http://localhost:1000/api/v1/users/register

Tramite questo end-point è possibile creare un utente passandogli la dto corretta nel body
della chiamata.

### Login

end-point -> POST: http://localhost:1000/api/v1/users/login

Per poter accedere a tutte le chiamate dell'API è necessario essere loggati correttamente.

Questo end-point permette di farlo fornendo username e password di un utente registrato.
La response della chiamata restituirà un token da usare per le altre operazioni.

### Aggiunta di un prodotto

end-point -> POST: http://localhost:1000/api/v1/products

Tramite questo end-point è possibile creare un prodotto passandogli la dto corretta nel body
della chiamata.

### Aggiunta di un ordine

end-point -> POST: http://localhost:1000/api/v1/orders

Tramite questo end-point è possibile creare un ordine con i relativi prodotti associati 
passandogli la dto corretta nel body della chiamata.

### ECC...

**N.B. Per comodità ho creato un seeder nel microservizio dei prodotti che genera automaticamente
alcuni dati**