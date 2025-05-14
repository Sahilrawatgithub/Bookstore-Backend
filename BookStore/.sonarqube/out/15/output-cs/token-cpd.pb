â
?D:\BridgeLabz\BookStore\BusinessLayer\Service\WishlistImplBL.cs
	namespace 	
BusinessLayer
 
. 
Service 
{ 
public 

class 
WishlistImplBL 
:  
IWishlistBL! ,
{ 
private 
readonly 
IWishlistRL $
_wishlistRL% 0
;0 1
public 
WishlistImplBL 
( 
IWishlistRL )

wishlistRL* 4
)4 5
{ 	
_wishlistRL 
= 

wishlistRL $
;$ %
} 	
public 
async 
Task 
< 
ResponseDTO %
<% &
string& ,
>, -
>- .
WishlistBookAsync/ @
(@ A
intA D
bookIdE K
,K L
intM P
userIdQ W
)W X
{ 	
return 
await 
_wishlistRL $
.$ %
WishlistBookAsync% 6
(6 7
bookId7 =
,= >
userId? E
)E F
;F G
} 	
public 
async 
Task 
< 
ResponseDTO %
<% &
List& *
<* +
WishListEntity+ 9
>9 :
>: ;
>; <&
GetAllWishlistedBooksAsync= W
(W X
intX [
userId\ b
)b c
{ 	
return 
await 
_wishlistRL $
.$ %&
GetAllWishlistedBooksAsync% ?
(? @
userId@ F
)F G
;G H
} 	
public 
async 
Task 
< 
ResponseDTO %
<% &
string& ,
>, -
>- .'
RemoveBookFromWishlistAsync/ J
(J K
intK N
bookIdO U
,U V
intW Z
userId[ a
)a b
{ 	
return 
await 
_wishlistRL $
.$ %'
RemoveBookFromWishlistAsync% @
(@ A
bookIdA G
,G H
userIdI O
)O P
;P Q
} 	
public   
async   
Task   
<   
ResponseDTO   %
<  % &
string  & ,
>  , -
>  - .
ClearWishlistAsync  / A
(  A B
int  B E
userId  F L
)  L M
{!! 	
return"" 
await"" 
_wishlistRL"" $
.""$ %
ClearWishlistAsync""% 7
(""7 8
userId""8 >
)""> ?
;""? @
}## 	
}%% 
}&& •
;D:\BridgeLabz\BookStore\BusinessLayer\Service\UserImplBL.cs
	namespace 	
BusinessLayer
 
. 
Service 
{ 
public 

class 

UserImplBL 
: 
IUserBL %
{ 
private 
readonly 
IUserRL  
userRl! '
;' (
public 

UserImplBL 
( 
IUserRL !
userRl" (
)( )
{ 	
this 
. 
userRl 
= 
userRl  
;  !
} 	
public 
async 
Task 
< 
ResponseDTO %
<% &
string& ,
>, -
>- .
RegisterUserAsync/ @
(@ A

RegUserDTOA K
requestL S
)S T
{ 	
return 
await 
userRl 
.  
RegisterUserAsync  1
(1 2
request2 9
)9 :
;: ;
} 	
public 
async 
Task 
< 
ResponseDTO %
<% &
LoginResponseDTO& 6
>6 7
>7 8

LoginAsync9 C
(C D
stringD J
emailK P
,P Q
stringR X
passwordY a
)a b
{ 	
return 
await 
userRl 
.  

LoginAsync  *
(* +
email+ 0
,0 1
password2 :
): ;
;; <
} 	
public 
async 
Task 
< 
ResponseDTO %
<% &
List& *
<* +

UserEntity+ 5
>5 6
>6 7
>7 8
GetAllUsersAsync9 I
(I J
)J K
{ 	
return 
await 
userRl 
.  
GetAllUsersAsync  0
(0 1
)1 2
;2 3
} 	
public   
async   
Task   
<   
ResponseDTO   %
<  % &
string  & ,
>  , -
>  - .
DeleteUserAsync  / >
(  > ?
string  ? E
email  F K
)  K L
{!! 	
return"" 
await"" 
userRl"" 
.""  
DeleteUserAsync""  /
(""/ 0
email""0 5
)""5 6
;""6 7
}## 	
public$$ 
async$$ 
Task$$ 
<$$ 
ResponseDTO$$ %
<$$% &
string$$& ,
>$$, -
>$$- .
ForgotPasswordAsync$$/ B
($$B C
string$$C I
email$$J O
)$$O P
{%% 	
return&& 
await&& 
userRl&& 
.&&  
ForgotPasswordAsync&&  3
(&&3 4
email&&4 9
)&&9 :
;&&: ;
}'' 	
public(( 
async(( 
Task(( 
<(( 
ResponseDTO(( %
<((% &
string((& ,
>((, -
>((- .
ResetPasswordAsync((/ A
(((A B
string((B H
email((I N
,((N O
string((P V
newPassword((W b
)((b c
{)) 	
return** 
await** 
userRl** 
.**  
ResetPasswordAsync**  2
(**2 3
email**3 8
,**8 9
newPassword**: E
)**E F
;**F G
}++ 	
},, 
}.. ‹
<D:\BridgeLabz\BookStore\BusinessLayer\Service\OrderImplBL.cs
	namespace 	
BusinessLayer
 
. 
Service 
{ 
public 

class 
OrderImplBL 
: 
IOrderBL '
{ 
private 
readonly 
IOrderRL !
orderRL" )
;) *
public 
OrderImplBL 
( 
IOrderRL #
orderRL$ +
)+ ,
{ 	
this 
. 
orderRL 
= 
orderRL "
;" #
} 	
public 
async 
Task 
< 
ResponseDTO %
<% &
List& *
<* +
OrderEntity+ 6
>6 7
>7 8
>8 9
GetAllOrdersAsync: K
(K L
intL O
userIdP V
)V W
{ 	
return 
await 
orderRL  
.  !
GetAllOrdersAsync! 2
(2 3
userId3 9
)9 :
;: ;
} 	
public 
async 
Task 
< 
ResponseDTO %
<% &
string& ,
>, -
>- .
OrderBookAsync/ =
(= >
OrderDTO> F
orderG L
,L M
intN Q
userIdR X
)X Y
{ 	
try 
{ 
return 
await 
orderRL $
.$ %
OrderBookAsync% 3
(3 4
order4 9
,9 :
userId; A
)A B
;B C
} 
catch   
(   
	Exception   
ex   
)    
{!! 
throw"" 
ex"" 
;"" 
}## 
}$$ 	
}%% 
}&& Ç-
;D:\BridgeLabz\BookStore\BusinessLayer\Service\CartImplBL.cs
	namespace

 	
BusinessLayer


 
.

 
Service

 
{ 
public 

class 

CartImplBL 
: 
ICartBL #
{ 
private 
readonly 
ICartRL  
cartRL! '
;' (
public 

CartImplBL 
( 
ICartRL !
cartRL" (
)( )
{ 	
this 
. 
cartRL 
= 
cartRL  
;  !
} 	
public 
async 
Task 
< 
ResponseDTO %
<% &
string& ,
>, -
>- .
AddToCartAsync/ =
(= >
AddToCartReqDTO> M
addToCartReqN Z
,Z [
int\ _
userId` f
)f g
{ 	
try 
{ 
return 
await 
cartRL #
.# $
AddToCartAsync$ 2
(2 3
addToCartReq3 ?
,? @
userIdA G
)G H
;H I
} 
catch 
( 
	Exception 
ex 
)  
{ 
return 
new 
ResponseDTO &
<& '
string' -
>- .
{ 
Success 
= 
false #
,# $
Message 
= 
ex  
.  !
Message! (
} 
; 
}   
}!! 	
public"" 
async"" 
Task"" 
<"" 
ResponseDTO"" %
<""% &
string""& ,
>"", -
>""- .
RemoveFromCartAsync""/ B
(""B C
int""C F
cartId""G M
,""M N
int""O R
userId""S Y
)""Y Z
{## 	
try$$ 
{%% 
return&& 
await&& 
cartRL&& #
.&&# $
RemoveFromCartAsync&&$ 7
(&&7 8
cartId&&8 >
,&&> ?
userId&&@ F
)&&F G
;&&G H
}'' 
catch(( 
((( 
	Exception(( 
ex(( 
)((  
{)) 
return** 
new** 
ResponseDTO** &
<**& '
string**' -
>**- .
{++ 
Success,, 
=,, 
false,, #
,,,# $
Message-- 
=-- 
ex--  
.--  !
Message--! (
}.. 
;.. 
}// 
}00 	
public11 
async11 
Task11 
<11 
ResponseDTO11 %
<11% &
List11& *
<11* +
CartResponseDTO11+ :
>11: ;
>11; <
>11< = 
GetAllCartItemsAsync11> R
(11R S
int11S V
userId11W ]
)11] ^
{22 	
try33 
{44 
return55 
await55 
cartRL55 #
.55# $ 
GetAllCartItemsAsync55$ 8
(558 9
userId559 ?
)55? @
;55@ A
}66 
catch77 
(77 
	Exception77 
ex77 
)77  
{88 
return99 
new99 
ResponseDTO99 &
<99& '
List99' +
<99+ ,
CartResponseDTO99, ;
>99; <
>99< =
{:: 
Success;; 
=;; 
false;; #
,;;# $
Message<< 
=<< 
ex<<  
.<<  !
Message<<! (
}== 
;== 
}>> 
}?? 	
public@@ 
async@@ 
Task@@ 
<@@ 
ResponseDTO@@ %
<@@% &
string@@& ,
>@@, -
>@@- .
UpdateCartAsync@@/ >
(@@> ?
int@@? B
cartId@@C I
,@@I J
int@@K N
quantity@@O W
,@@W X
int@@Y \
userId@@] c
)@@c d
{AA 	
tryBB 
{CC 
returnDD 
awaitDD 
cartRLDD #
.DD# $
UpdateCartAsyncDD$ 3
(DD3 4
cartIdDD4 :
,DD: ;
quantityDD< D
,DDD E
userIdDDF L
)DDL M
;DDM N
}EE 
catchFF 
(FF 
	ExceptionFF 
exFF 
)FF  
{GG 
returnHH 
newHH 
ResponseDTOHH &
<HH& '
stringHH' -
>HH- .
{HH/ 0
SuccessHH1 8
=HH9 :
falseHH; @
,HH@ A
MessageHHB I
=HHJ K
exHHL N
.HHN O
MessageHHO V
}HHW X
;HHX Y
}II 
}JJ 	
publicKK 
asyncKK 
TaskKK 
<KK 
ResponseDTOKK %
<KK% &
stringKK& ,
>KK, -
>KK- .
ClearCartAsyncKK/ =
(KK= >
intKK> A
userIdKKB H
)KKH I
{LL 	
tryMM 
{NN 
returnOO 
awaitOO 
cartRLOO #
.OO# $
ClearCartAsyncOO$ 2
(OO2 3
userIdOO3 9
)OO9 :
;OO: ;
}PP 
catchQQ 
(QQ 
	ExceptionQQ 
exQQ 
)QQ  
{RR 
returnSS 
newSS 
ResponseDTOSS &
<SS& '
stringSS' -
>SS- .
{TT 
SuccessUU 
=UU 
falseUU #
,UU# $
MessageVV 
=VV 
exVV  
.VV  !
MessageVV! (
}WW 
;WW 
}XX 
}YY 	
}ZZ 
}[[ «
;D:\BridgeLabz\BookStore\BusinessLayer\Service\BookImplBL.cs
	namespace 	
BusinessLayer
 
. 
Service 
{ 
public 

class 

BookImplBL 
: 
IBookBL #
{ 
private 
readonly 
IBookRL  
_bookRL! (
;( )
public 

BookImplBL 
( 
IBookRL !
bookRl" (
)( )
{ 	
_bookRL 
= 
bookRl 
; 
} 	
public 
async 
Task 
< 
ResponseDTO %
<% &
string& ,
>, -
>- .
UploadBookAsync/ >
(> ?
AddBookReqDTO? L
requestM T
,T U
intU X
userIdY _
)_ `
{ 	
return 
await 
_bookRL  
.  !
UploadBookAsync! 0
(0 1
request1 8
,8 9
userId9 ?
)? @
;@ A
} 	
public 
async 
Task 
< 
ResponseDTO %
<% &

BookEntity& 0
>0 1
>1 2
ViewBookByIdAsync3 D
(D E
intE H
bookIdI O
)O P
{ 	
return 
await 
_bookRL  
.  !
ViewBookByIdAsync! 2
(2 3
bookId3 9
)9 :
;: ;
} 	
public 
async 
Task 
< 
ResponseDTO %
<% &
List& *
<* +

BookEntity+ 5
>5 6
>6 7
>7 8
GetAllBooksAsync9 I
(I J
)J K
{ 	
return 
await 
_bookRL  
.  !
GetAllBooksAsync! 1
(1 2
)2 3
;3 4
} 	
}   
}!! !
>D:\BridgeLabz\BookStore\BusinessLayer\Service\AddressImplBL.cs
	namespace 	
BusinessLayer
 
. 
Service 
{ 
public 

class 
AddressImplBL 
: 

IAddressBL )
{ 
private 
readonly 

IAddressRL #

_addressRL$ .
;. /
public 
AddressImplBL 
( 

IAddressRL '
	addressRL( 1
)1 2
{ 	

_addressRL 
= 
	addressRL "
;" #
} 	
public 
async 
Task 
< 
ResponseDTO %
<% &
string& ,
>, -
>- .
AddAddressAsync/ >
(> ?
UserAddressReqDTO? P
requestQ X
,X Y
intZ ]
userId^ d
)d e
{ 	
try 
{ 
return 
await 

_addressRL '
.' (
AddAddressAsync( 7
(7 8
request8 ?
,? @
userIdA G
)G H
;H I
} 
catch 
( 
	Exception 
ex 
)  
{ 
throw 
new 
	Exception #
(# $
ex$ &
.& '
Message' .
). /
;/ 0
} 
} 	
public 
async 
Task 
< 
ResponseDTO %
<% &
string& ,
>, -
>- .
UpdateAddressAsync/ A
(A B
UserAddressReqDTOB S
requestT [
,[ \
int\ _
adressId` h
,h i
intj m
userIdn t
)t u
{   	
try!! 
{"" 
return## 
await## 

_addressRL## '
.##' (
UpdateAddressAsync##( :
(##: ;
request##; B
,##B C
adressId##C K
,##K L
userId##M S
)##S T
;##T U
}$$ 
catch%% 
(%% 
	Exception%% 
ex%% 
)%%  
{&& 
throw'' 
new'' 
	Exception'' #
(''# $
ex''$ &
.''& '
Message''' .
)''. /
;''/ 0
}(( 
})) 	
public** 
async** 
Task** 
<** 
ResponseDTO** %
<**% &
string**& ,
>**, -
>**- .
DeleteAddressAsync**/ A
(**A B
int**B E
	addressId**F O
,**O P
int**Q T
userId**U [
)**[ \
{++ 	
try,, 
{-- 
return.. 
await.. 

_addressRL.. '
...' (
DeleteAddressAsync..( :
(..: ;
	addressId..; D
,..D E
userId..F L
)..L M
;..M N
}// 
catch00 
(00 
	Exception00 
ex00 
)00  
{11 
throw22 
new22 
	Exception22 #
(22# $
ex22$ &
.22& '
Message22' .
)22. /
;22/ 0
}33 
}44 	
public55 
async55 
Task55 
<55 
ResponseDTO55 %
<55% &
List55& *
<55* +
AddressEntity55+ 8
>558 9
>559 :
>55: ; 
GetAllAddressesAsync55< P
(55P Q
int55Q T
userId55U [
)55[ \
{66 	
try77 
{88 
return99 
await99 

_addressRL99 '
.99' ( 
GetAllAddressesAsync99( <
(99< =
userId99= C
)99C D
;99D E
}:: 
catch;; 
(;; 
	Exception;; 
ex;; 
);;  
{<< 
throw== 
new== 
	Exception== #
(==# $
ex==$ &
.==& '
Message==' .
)==. /
;==/ 0
}>> 
}?? 	
}@@ 
}AA Æ
>D:\BridgeLabz\BookStore\BusinessLayer\Interface\IWishlistBL.cs
	namespace		 	
BusinessLayer		
 
.		 
	Interface		 !
{

 
public 

	interface 
IWishlistBL  
{ 
public 
Task 
< 
ResponseDTO 
<  
string  &
>& '
>' (
WishlistBookAsync) :
(: ;
int; >
bookId? E
,E F
intG J
userIdK Q
)Q R
;R S
public 
Task 
< 
ResponseDTO 
<  
List  $
<$ %
WishListEntity% 3
>3 4
>4 5
>5 6&
GetAllWishlistedBooksAsync7 Q
(Q R
intR U
userIdV \
)\ ]
;] ^
public 
Task 
< 
ResponseDTO 
<  
string  &
>& '
>' ('
RemoveBookFromWishlistAsync) D
(D E
intE H
bookIdI O
,O P
intQ T
userIdU [
)[ \
;\ ]
public 
Task 
< 
ResponseDTO 
<  
string  &
>& '
>' (
ClearWishlistAsync) ;
(; <
int< ?
userId@ F
)F G
;G H
} 
} ¡
:D:\BridgeLabz\BookStore\BusinessLayer\Interface\IUserBL.cs
	namespace		 	
BusinessLayer		
 
.		 
	Interface		 !
{

 
public 

	interface 
IUserBL 
{ 
public 
Task 
< 
ResponseDTO 
<  
string  &
>& '
>' (
RegisterUserAsync) :
(: ;

RegUserDTO; E
requestF M
)M N
;N O
public 
Task 
< 
ResponseDTO 
<  
LoginResponseDTO  0
>0 1
>1 2

LoginAsync3 =
(= >
string> D
emailE J
,J K
stringL R
passwordS [
)[ \
;\ ]
public 
Task 
< 
ResponseDTO 
<  
List  $
<$ %

UserEntity% /
>/ 0
>0 1
>1 2
GetAllUsersAsync3 C
(C D
)D E
;E F
public 
Task 
< 
ResponseDTO 
<  
string  &
>& '
>' (
DeleteUserAsync) 8
(8 9
string9 ?
email@ E
)E F
;F G
public 
Task 
< 
ResponseDTO 
<  
string  &
>& '
>' (
ForgotPasswordAsync) <
(< =
string= C
emailD I
)I J
;J K
public 
Task 
< 
ResponseDTO 
<  
string  &
>& '
>' (
ResetPasswordAsync) ;
(; <
string< B
emailC H
,H I
stringJ P
newPasswordQ \
)\ ]
;] ^
} 
} ö
;D:\BridgeLabz\BookStore\BusinessLayer\Interface\IOrderBL.cs
	namespace		 	
BusinessLayer		
 
.		 
	Interface		 !
{

 
public 

	interface 
IOrderBL 
{ 
public 
Task 
< 
ResponseDTO 
<  
string  &
>& '
>' (
OrderBookAsync) 7
(7 8
OrderDTO8 @
orderA F
,F G
intH K
userIdL R
)R S
;S T
public 
Task 
< 
ResponseDTO 
<  
List  $
<$ %
OrderEntity% 0
>0 1
>1 2
>2 3
GetAllOrdersAsync4 E
(E F
intF I
userIdJ P
)P Q
;Q R
} 
} §
:D:\BridgeLabz\BookStore\BusinessLayer\Interface\ICartBL.cs
	namespace 	
BusinessLayer
 
. 
	Interface !
{		 
public

 

	interface

 
ICartBL

 
{ 
public 
Task 
< 
ResponseDTO 
<  
string  &
>& '
>' (
AddToCartAsync) 7
(7 8
AddToCartReqDTO8 G
addToCartReqDTOH W
,W X
intY \
userId] c
)c d
;d e
public 
Task 
< 
ResponseDTO 
<  
string  &
>& '
>' (
RemoveFromCartAsync) <
(< =
int= @
cartIdA G
,G H
intI L
userIdM S
)S T
;T U
public 
Task 
< 
ResponseDTO 
<  
List  $
<$ %
CartResponseDTO% 4
>4 5
>5 6
>6 7 
GetAllCartItemsAsync8 L
(L M
intM P
userIdQ W
)W X
;X Y
public 
Task 
< 
ResponseDTO 
<  
string  &
>& '
>' (
UpdateCartAsync) 8
(8 9
int9 <
cartId= C
,C D
intE H
quantityI Q
,Q R
intS V
userIdW ]
)] ^
;^ _
public 
Task 
< 
ResponseDTO 
<  
string  &
>& '
>' (
ClearCartAsync) 7
(7 8
int8 ;
userId< B
)B C
;C D
} 
} Ù
:D:\BridgeLabz\BookStore\BusinessLayer\Interface\IBookBL.cs
	namespace		 	
BusinessLayer		
 
.		 
	Interface		 !
{

 
public 

	interface 
IBookBL 
{ 
public 
Task 
< 
ResponseDTO 
<  
string  &
>& '
>' (
UploadBookAsync) 8
(8 9
AddBookReqDTO9 F
requestG N
,N O
intO R
userIdS Y
)Y Z
;Z [
public 
Task 
< 
ResponseDTO 
<  

BookEntity  *
>* +
>+ ,
ViewBookByIdAsync- >
(> ?
int? B
bookIdC I
)I J
;J K
public 
Task 
< 
ResponseDTO 
<  
List  $
<$ %

BookEntity% /
>/ 0
>0 1
>1 2
GetAllBooksAsync3 C
(C D
)D E
;E F
} 
} ¾
=D:\BridgeLabz\BookStore\BusinessLayer\Interface\IAddressBL.cs
	namespace		 	
BusinessLayer		
 
.		 
	Interface		 !
{

 
public 

	interface 

IAddressBL 
{ 
public 
Task 
< 
ResponseDTO 
<  
string  &
>& '
>' (
AddAddressAsync) 8
(8 9
UserAddressReqDTO9 J
requestK R
,R S
intT W
userIdX ^
)^ _
;_ `
public 
Task 
< 
ResponseDTO 
<  
string  &
>& '
>' (
UpdateAddressAsync) ;
(; <
UserAddressReqDTO< M
requestN U
,U V
intV Y
	addressIdZ c
,c d
inte h
userIdi o
)o p
;p q
public 
Task 
< 
ResponseDTO 
<  
string  &
>& '
>' (
DeleteAddressAsync) ;
(; <
int< ?
	addressId@ I
,I J
intK N
userIdO U
)U V
;V W
public 
Task 
< 
ResponseDTO 
<  
List  $
<$ %
AddressEntity% 2
>2 3
>3 4
>4 5 
GetAllAddressesAsync6 J
(J K
intK N
userIdO U
)U V
;V W
} 
} 