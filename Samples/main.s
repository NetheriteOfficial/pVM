; retrive health
call 0
; push
push 2
; decrease
sub
; set health
call 1
; jump to end if carry
jmz 26
; loop to beggining
jmp 0
; halt execution
halt