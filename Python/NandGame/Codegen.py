# Ибо писать руками заипёшься

indent = 0


def Comment(*comments):
    global indent
    print(f"""{'    '*indent}# {' '.join(comments)}""")


def SetVar(var, value):
    global indent
    print(f"""
{'    '*indent}# var.{var} = {value}
{'    '*indent}A = {value}
{'    '*indent}D = A
{'    '*indent}A = {var}
{'    '*indent}*A = D""")


def Call(retaddr, func, retptr):
    global indent
    print(f"""
{'    '*indent}# Call func:{func} -> {retptr}
{'    '*indent}A = {retptr}
{'    '*indent}D = A
{'    '*indent}A = {retaddr}
{'    '*indent}*A = D
{'    '*indent}A = {func}
{'    '*indent}JMP
{'    '*indent}LABEL {retptr}""")


def CheckBitAndJump(var, flag, dest):
    global indent
    print(f"""
{'    '*indent}# var.{var} ? {flag} -> {dest}
{'    '*indent}A = {var}
{'    '*indent}D = *A
{'    '*indent}A = {flag}
{'    '*indent}D = D&A
{'    '*indent}A = {dest}
{'    '*indent}D ; JNE""")


def Funct(name):
    global indent
    print(f"""

# function {name}
LABEL {name}""")
    indent += 1



def End(retaddr):
    global indent
    print(f"""
{'    '*indent}A = {retaddr}
{'    '*indent}A = *A
{'    '*indent}JMP
# end of function

""")
    indent -= 1


def Asm(*args):
    global indent
    print(f"{'    '*indent}{' '.join(args)}")


def Goto(label):
    global indent
    print(f"""
{'    '*indent}# goto {label}
{'    '*indent}A = {label}
{'    '*indent}JMP""")


def JumpIfZero(var, label):
    global indent
    print(f"""
{'    '*indent}# var.{var} == 0 ? {label} : next
{'    '*indent}A = {var}
{'    '*indent}D = *A
{'    '*indent}A = {label}
{'    '*indent}D ; JEQ""")


def Compile(prog: str):
    commands = {
        "#": Comment,
        "var": SetVar,
        "call": Call,
        "check": CheckBitAndJump,
        "fun": Funct,
        "end": End,
        "goto": Goto,
        "ifz": JumpIfZero,
        ":": Asm
    }
    for line in prog.splitlines():
        if line == '':
            continue
        params = line.lstrip().split()
        try:
            cmd, args = params[0], params[1:]
            cmd = commands[cmd]
            cmd(*args)
        except:
            print(f"AAAAAAAA:\n{line} -> {params}\n\n")
            return


def main():
    Compile("""
# sublang
# Переменные:
#   0 - номер стены
#   1 - стена слева
#   2 - стена прямо
#   3 - стена справа
#   100 - возврат из подпрограммы
fun MAIN
    var 0 0
    var 100 0

    : # Сначала находим все стенки
    call 100 TURN_LEFT MAIN_1
    var 0 1
    call 100 CHECK_WALL MAIN_2
    call 100 TURN_RIGHT MAIN_3
    var 0 2
    call 100 CHECK_WALL MAIN_4
    call 100 TURN_RIGHT MAIN_5
    var 0 3
    call 100 CHECK_WALL MAIN_6
    call 100 TURN_LEFT MAIN_7

    : # теперь решаем в какую сторону ехать, по правилу правой руки
    ifz 3 MAIN_MOVE_RIGHT
    ifz 2 MAIN_MOVE_FORWARD
    ifz 1 MAIN_MOVE_LEFT
    goto MOVE_BACK

    : LABEL MAIN_MOVE_RIGHT
    call 100 TURN_RIGHT MAIN_8
    call 100 MOVE_FORWARD MAIN_9
    goto END_OF_VARIANTS

    : LABEL MAIN_MOVE_FORWARD
    call 100 MOVE_FORWARD MAIN_10
    goto END_OF_VARIANTS

    : LABEL MAIN_MOVE_LEFT
    call 100 TURN_LEFT MAIN_11
    call 100 MOVE_FORWARD MAIN_12
    goto END_OF_VARIANTS

    : LABEL MOVE_BACK
    call 100 TURN_LEFT MAIN_13
    call 100 TURN_LEFT MAIN_14
    call 100 MOVE_FORWARD MAIN_15
    goto END_OF_VARIANTS

    : LABEL END_OF_VARIANTS
    goto MAIN
    call 100 FULL_END MAIN_END
end 100

fun CHECK_WALL
    : A = 0
    : A = *A
    : *A = 0
    check 0x7FFF 0b0000_0001_0000_0000 CHECK_WALL_DETECTED
    goto END_CHECK_WALL
    : LABEL CHECK_WALL_DETECTED
    : A = 0
    : A = *A
    : *A = 1
    : LABEL END_CHECK_WALL
end 100

fun TURN_LEFT
    var 0x7FFF 0b0000_1000
    : LABEL TURN_LEFT_PROGRESS
    check 0x7FFF 0b0000_0010_0000_0000 TURN_LEFT_PROGRESS
end 100

fun TURN_RIGHT
    var 0x7FFF 0b0001_0000
    : LABEL TURN_RIGHT_PROGRESS
    check 0x7FFF 0b0000_0010_0000_0000 TURN_RIGHT_PROGRESS
end 100

fun MOVE_FORWARD
    var 0x7FFF 0b0000_0100
    : LABEL MOVE_FORWARD_PROGRESS
    check 0x7FFF 0b0000_0100_0000_0000 MOVE_FORWARD_PROGRESS
end 100

: LABEL FULL_END
""")


if __name__ == "__main__":
    main()
