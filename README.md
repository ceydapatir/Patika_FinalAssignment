# Patika - VakıfBank FullStack Bootcamp Final Projesi
Final projesi kapsamında .NET tabanlı bir WebAPI projesi gerçekleştirdim. Kullanıcı arayüzünü ise Angular kullanılarak geliştirdim. Proje temelde web tabanlı bir bayi yönetim uygulamasıdır. Bu bakımdan proje tedarikçi firmanın bayilerini kaydedip takip edebildiği, sipariş ve ürün detaylarını görebilmesini sağlarken, bayilerin ise tedarikçi firma ürünlerini inceleyerek sipariş oluşturmasını sağlamaktadır. 

Uygulama içerisinde satın alma, ürün takip, sipariş onayı ve takibi işlemleri şirket çalışanları tarafından sağlanmaktadır. Şirket çalışanları "admin" bayi çalışanları ise "bayi" rolündedir. Admin kullanıcılar yeni bayi ekleme, sipariş onaylama ve çeşitli yönetim işlemleri yapabilmektedirler. 

Proje Unit Of Work design pattern ile tasarlandı. Tüm modellerin POST ve PUT istekleri için Fluent Validation uygulandı. Proje içerisinde AutoMapper ve MediatR kullanıldı. .NET Web API tarafında JSON Web Token (JWT) tabanlı kimlik doğrulama ve yetkilendirme kullanılarak güvenlik sağlandı.


---
## Requests

***PUT requestler null değer verilen datalar için güncelleme yapmaz. Yalnızca kullanıcının güncellemek için girdiği değişkenleri günceller.***

***Proje bir mobil bankacılık uyguluması olarak verilmediği için kart bilgileri databasede bağımsız tablolarca tutulmakta ve yalnızca ödeme işlemleri için istek gönderilmektedir.***

### AddressesController
* GET   /api/address: İstekte bulunan kullanıcının firma adresini return eder.
* POST  /api/address: İstekte bulunan kullanıcının firmasına ait adres oluşturur.
* PUT   /api/address: İstekte bulunan kullanıcının firma adresini alınan verilerle günceller.
* DELETE   /api/address: İstekte bulunan kullanıcının firma adresini siler.
* GET   /api/companies/{company_id}/address: company_id ye sahip firmanın adresini return eder.


***Kart bilgileri ödeme kolaylaylığı için yalnızca "Kart Adı" ve "Kart Numara" halinde tutulduğundan PUT request uygulanmamıştır.***
### CardsController
* GET   /api/cards: İstekte bulunan kullanıcının firmasına ait kartları return eder.
* POST  /api/cards: İstekte bulunan kullanıcının firmasına ait kartı oluşturur.
* DELETE   /api/cards/{card_id}: card_id ye sahip adresi siler.
* GET   /api/cards/{card_id}: card_id ye sahip kartı return eder.

### DealersController
* GET   /api/dealer: İstekte bulunan dealer kullanıcının firma bilgilerini return eder.
* PUT   /api/dealer: İstekte bulunan dealer kullanıcının firma bilgilerini alınan verilerle günceller.
* GET   /api/dealers: İstekte bulunan admin kullanıcının firmasına bağlı bayileri return eder.
* POST  /api/address: İstekte bulunan admin kullanıcının firmasına bağlı bayi oluşturur.
* GET   /api/dealers/{company_id}: company_id ye sahip bayiyi return eder.
* POST  /api/address/{company_id}: company_id ye sahip bayiyi alınan verilerle günceller.
* DELETE   /api/dealers/{company_id}: company_id ye sahip bayiyi siler.

### EmployeesController
* GET   /api/employee: İstekte bulunan kullanıcının hesap bilgilerini return eder.
* POST  /api/employee: Yeni kullanıcı oluşturur.
* PUT   /api/employee: İstekte bulunan kullanıcının hesap bilgilerini alınan verilerle günceller.
* GET   /api/employees/{employee_id}: employee_id ye sahip çalışanı return eder.
* DELETE   /api/employees/{employee_id}: employee_id ye sahip çalışanı siler.
* GET   /api/companies/{company_id}/employees: company_id ye sahip firmaya bağlı çalışanları return eder.

### MessagesController
* GET   /api/messages: İstekte bulunan kullanıcıya ait mesajları return eder.
* POST  /api/messages: İstekte bulunan kullanıcıya ait mesaj oluşturur.
* DELETE   /api/messages/{message_id}: message_id ye sahip mesajı siler.


***Sepet ürünlerinde yapılan her değişiklik ürün stoklarına göre ve sepeti güncelleyecek şekilde ayarlanmıştır.***
### OrderItemsController
* GET   /api/order-items: İstekte bulunan kullanıcıya ait sepetteki ürünleri return eder.
* POST  /api/order-items: İstekte bulunan kullanıcıya ait sepete ürün ekler.
* DELETE   /api/order-items: İstekte bulunan kullanıcıya ait sepetteki tüm ürünleri siler.
* PUT  /api/order-items/{orderitem_id}: orderitem_id ye sahip sepet ürününün miktarını alınan veriyle günceller.
* DELETE  /api/order-items/{orderitem_id}: orderitem_id ye sahip ürürü sepetten siler.
* GET  /api/orders/{order_id}/order-items: order_id ye sahip siparişe ait ürünleri return eder.


***Order modeli sepet ve sipariş olarak kullanılmakta ve bu durum status değişkeni ile yönetilmektedir.***

***Ödeme işlemi yapıldıktan sonra sepet sipraiş olarak güncellenir ve tedarikçi onayı beklenir.***

***Sipariş ödeme tamamlandıktan veya onaylandıktan sonra iptal edilemez. Sipariş onay veya iptaline kadar ürün stokta rezerv edilir ve ödeme tedarikçiye geçmez.***

***Sipariş ödeme tamamlandıktan veya onaylandıktan sonra iptal edilemez. Sipariş iptalinde bayiye ücret iadesi yapılır ve stok rezervi iptal edilir.***

### OrdersController
* GET   /api/cart: İstekte bulunan kullanıcıya ait sepeti (Birim fiyat toplamı, KDV topplamı, Ödenecek Tutar vb.) return eder.
* GET   /api/orders: İstekte bulunan kullanıcının firmasına ait siparişleri return eder.
* GET   /api/orders/{order_id}: order_id ye sahip siparişi return eder
* PUT  /api/orders/{order_id}: order_id ye sahip siparişi onaylar.
* DELETE  /api/orders/{order_id}: order_id ye sahip siparişi iptal eder.


***Tüm ödeme işlemleri veritabanındaki bağımsız tablolara yapılan istekler ile sağlanır. Proje kapsamı gereği hesap ve kart bilgileri esas tablolara dahil edilmemiştir.***
### PaymentsController
* GET   /api/orders/{order_id}/payment: order_id ye sahip siparişin ödeme bilgilerini return eder.
* POST  /api/payment: İstekte bulunan kullanıcıya ait sepetin ödemesini oluşturur.

### ProductsController
* GET   /api/products: İstekte bulunan kullanıcının firmasına ait ürünleri return eder.
* POST  /api/products: İstekte bulunan admin kullanıcının firmasına ait ürün oluşturur.
* PUT   /api/products/{product_id}: product_id ye sahip ürünü alınan verlerle günceller.
* DELETE  /api/products/{product_id}: product_id ye sahip ürünü siler.
* GET   /api/supplier/products: İstekte bulunan dealer kullanıcının tedarikçisine ait ürünleri return eder.
* GET   /api/dealers/{company_id}/products: company_id ye sahip bayiye ait ürünleri return eder.
  

***Proje alt yapısı birden fazla tedarikçi için kurgulandığı için gerektiği takdirde POST ve DELETE requestler eklenerek birden fazla tedarikçi ile çalışmak mümkün.***
### SuppliersController
* GET   /api/supplier: İstekte bulunan admin kullanıcının tedarikçi bilgilerini admin kullanıcının ise firma bilgilerini return eder.
* PUT  /api/supplier: İstekte bulunan admin kullanıcının tedarikçi bilgilerini alınan veriler ile günceller.

  
