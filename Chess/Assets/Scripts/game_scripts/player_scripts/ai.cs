u s i n g   S y s t e m . C o l l e c t i o n s . G e n e r i c ;  
 u s i n g   C h e s s G l o b a l s ;  
 p u b l i c   c l a s s   A I :   P l a y e r {  
 	 p r i v a t e   i n t   d e p t h ;  
     	 p r i v a t e   C h e s s G a m e C o n t r o l l e r   c h e s s G a m e ;  
     	 p u b l i c   A I ( C h e s s G a m e C o n t r o l l e r   c h e s s G a m e ,   i n t   d e p t h ) / / R u l e s   n o w   i n   e a c h   p i e c e  
 	 {  
 	 }  
     	 p u b l i c   o v e r r i d e   M o v e   g e t M o v e ( )  
 	 {  
 	 	 r e t u r n   M o v e ( ) ;  
 	 }  
 	 p u b l i c   o v e r r i d e   i n t   g e t I d ( )  
 	 {  
 	 	 r e t u r n   0 ;  
 	 }  
 	 p u b l i c   o v e r r i d e   c h a r   g e t C o l o r ( )  
 	 {  
 	 	 r e t u r n   ' 0 ' ;  
 	 }  
     	 p u b l i c   o v e r r i d e   b o o l   m o v e S u c c e s s f u l l y E x c e c u t e d ( )  
 	 {  
 	 	 r e t u r n   f a l s e ;  
 	 }  
     	 p u b l i c   M o v e   g e t B e s t M o v e ( )  
 	 {  
 	 	 r e t u r n   n e w   M o v e   ( ) ;  
 	 }  
     	 p u b l i c   v o i d   D o M o v e ( M o v e   m o v e )  
 	 {  
 	 }  
     	 p u b l i c   L i s t < M o v e >   g e n e r a t e M o v e s ( )  
 	 {  
 	 	 r e t u r n   n e w   L i s t < M o v e >   ( ) ;  
 	 }  
     	 p u b l i c   i n t   e v a l u a t e G a m e S t a t e ( )  
 	 {  
 	 	 r e t u r n   0 ;  
 	 }  
     	 p u b l i c   i n t   g e t S c o r e F o r S q u a r e ( P o s i t i o n   s q u a r e )  
 	 {  
 	 	 r e t u r n   0 ;  
 	 }  
     	 p u b l i c   i n t   g e t S c o r e F o r P i e c e T y p e ( P I E C E _ T Y P E S   t y p e )  
 	 {  
 	 	 r e t u r n   0 ;  
 	 }  
     	 p u b l i c   i n t   n e g M a x ( i n t   d e p t h )  
 	 {  
 	 	 r e t u r n   0 ;  
 	 }  
     	  
 }  
 