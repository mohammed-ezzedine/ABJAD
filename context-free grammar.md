program             →   binding_list
                    |   ε ;

binding_list        →   binding_list binding
                    |   binding ;

binding             →   statement
                    |   declaration ;

statement           →   expr_stmt
                    |   if_stmt
                    |   while_stmt
                    |   for_stmt
                    |   return_stmt
                    |   assignment_stmt ;

declaration         →   FUNC ID OPEN_PAREN parameter_list CLOSE_PAREN block
                    |   CONST ID EQUAL expression SEMICOLON
                    |   VAR ID EQUAL expression SEMICOLON
                    |   CLASS ID block ;

expr_stmt           →   expression SEMICOLON ;

if_stmt             →   IF OPEN_PAREN expression CLOSE_PAREN block ;

while_stmt          →   WHILE OPEN_PAREN expression CLOSE_PAREN block ;

for_stmt            →   FOR OPEN_PAREN declaration statement statement CLOSE_PAREN block ;

return_stmt         →   RETURN expression SEMICOLON ;

assignment_stmt     →   ID EQUAL expression SEMICOLON ;

parameter_list      →   primitive_list
                    |   ε ;

block               →   OPEN_BRACE binding_list CLOSE_BRACE ;

expression          →   call_expr
                    |   instant_expr
                    |   oper_expr
                    |   primitive ;

primitive_list      →   primitive_list COMMA primitive
                    |   primitive;

call_expr           →   ID DOT ID OPEN_PAREN parameter_list CLOSE_PAREN
                    |   ID OPEN_PAREN parameter_list CLOSE_PAREN ;

instant_expr        →   NEW ID OPEN_PAREN parameter_List CLOSE_PAREN ;

oper_expr           →   expression PLUS expression
                    |   expression MINUS expression
                    |   expression TIMES expression
                    |   expression DIVIDED_BY expression
                    |   expression AND expression
                    |   expression OR expression
                    |   expression EQUAL_EQUAL expression
                    |   expression GREATER_THAN expression
                    |   expression GREATER_EQUAL expression
                    |   expression LESS_THAN expression
                    |   expression LESS_EQUAL expression
                    |   expression BANG_EQUAL expression
                    |   BANG expression
                    |   MINUS expression ;

primitive           →   INT_CONST
                    |   FLOAT_CONST
                    |   STRING_CONST
                    |   TRUE
                    |   FALSE
                    |   NULL
                    |   ID ;