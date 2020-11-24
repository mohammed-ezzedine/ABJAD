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
                        |   block_stmt
                        |   return_stmt
                        |   assignment_stmt 
                        |   print_stmt
                        |   comment_stmt;

    declaration         →   FUNC ID OPEN_PAREN parameter_list CLOSE_PAREN block_stmt
                        |   CONST ID EQUAL expression SEMICOLON
                        |   VAR ID EQUAL expression SEMICOLON
                        |   CLASS ID block_stmt ;

    expr_stmt           →   expression SEMICOLON ;

    if_stmt             →   if_clause
                        |   if_clause else_clause ;

    while_stmt          →   WHILE OPEN_PAREN expression CLOSE_PAREN block_stmt ;

    for_stmt            →   FOR OPEN_PAREN declaration expression SEMICOLON expression CLOSE_PAREN block_stmt ;

    block_stmt          →   OPEN_BRACE binding_list CLOSE_BRACE ;

    return_stmt         →   RETURN expression SEMICOLON ;

    assignment_stmt     →   ID EQUAL expression SEMICOLON ;

    print_stmt          →   PRINT OPEN_PAREN expression CLOSE_PAREN SEMICOLON ;

    parameter_list      →   expr_list
                        |   ε ;

    if_clause           →   IF OPEN_PAREN expression CLOSE_PAREN block_stmt ;

    else_clause         →  ELSE block_stmt

    expression          →   call_expr
                        |   instant_expr
                        |   oper_expr
                        |   to_str_expr
                        |   to_number_expr
                        |   to_bool_expr
                        |   type_expr
                        |   primitive 
                        |   OPEN_PAREN expression CLOSE_PAREN ;

    expr_list           →   expr_list COMMA expression
                        |   expression ;

    call_expr           →   ID DOT ID OPEN_PAREN parameter_list CLOSE_PAREN SEMICOLON
                        |   ID OPEN_PAREN parameter_list CLOSE_PAREN SEMICOLON ;

    instant_expr        →   NEW ID OPEN_PAREN parameter_List CLOSE_PAREN ;

    to_str_expr         →   STRING OPEN_PAREN expression CLOSE_PAREN SEMICOLON ;

    to_number_expr      →   NUMBER OPEN_PAREN expression CLOSE_PAREN SEMICOLON ;

    to_bool_expr        →   BOOL OPEN_PAREN expression CLOSE_PAREN SEMICOLON ;

    type_stmt           →   TYPEOF OPEN_PAREN expression CLOSE_PAREN SEMICOLON ;

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

    primitive           →   NUMBER_CONST
                        |   STRING_CONST
                        |   TRUE
                        |   FALSE
                        |   NULL
                        |   ID ;
