/* A Bison parser, made by GNU Bison 3.5.  */

/* Bison interface for Yacc-like parsers in C

   Copyright (C) 1984, 1989-1990, 2000-2015, 2018-2019 Free Software Foundation,
   Inc.

   This program is free software: you can redistribute it and/or modify
   it under the terms of the GNU General Public License as published by
   the Free Software Foundation, either version 3 of the License, or
   (at your option) any later version.

   This program is distributed in the hope that it will be useful,
   but WITHOUT ANY WARRANTY; without even the implied warranty of
   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
   GNU General Public License for more details.

   You should have received a copy of the GNU General Public License
   along with this program.  If not, see <http://www.gnu.org/licenses/>.  */

/* As a special exception, you may create a larger work that contains
   part or all of the Bison parser skeleton and distribute that work
   under terms of your choice, so long as that work isn't itself a
   parser generator using the skeleton or a modified version thereof
   as a parser skeleton.  Alternatively, if you modify or redistribute
   the parser skeleton itself, you may (at your option) remove this
   special exception, which will cause the skeleton and the resulting
   Bison output files to be licensed under the GNU General Public
   License without this special exception.

   This special exception was added by the Free Software Foundation in
   version 2.2 of Bison.  */

/* Undocumented macros, especially those whose name start with YY_,
   are private implementation details.  Do not rely on them.  */

#ifndef YY_BASE_YY_GRAM_H_INCLUDED
# define YY_BASE_YY_GRAM_H_INCLUDED
/* Debug traces.  */
#ifndef YYDEBUG
# define YYDEBUG 0
#endif
#if YYDEBUG
extern int base_yydebug;
#endif

/* Token type.  */
#ifndef YYTOKENTYPE
# define YYTOKENTYPE
  enum yytokentype
  {
    IDENT = 258,
    UIDENT = 259,
    FCONST = 260,
    SCONST = 261,
    USCONST = 262,
    BCONST = 263,
    XCONST = 264,
    Op = 265,
    ICONST = 266,
    PARAM = 267,
    TYPECAST = 268,
    DOT_DOT = 269,
    COLON_EQUALS = 270,
    EQUALS_GREATER = 271,
    LESS_EQUALS = 272,
    GREATER_EQUALS = 273,
    NOT_EQUALS = 274,
    ABORT_P = 275,
    ABSOLUTE_P = 276,
    ACCESS = 277,
    ACTION = 278,
    ADD_P = 279,
    ADMIN = 280,
    AFTER = 281,
    AGGREGATE = 282,
    ALL = 283,
    ALSO = 284,
    ALTER = 285,
    ALWAYS = 286,
    ANALYSE = 287,
    ANALYZE = 288,
    AND = 289,
    ANY = 290,
    ARRAY = 291,
    AS = 292,
    ASC = 293,
    ASSERTION = 294,
    ASSIGNMENT = 295,
    ASYMMETRIC = 296,
    AT = 297,
    ATTACH = 298,
    ATTRIBUTE = 299,
    AUTHORIZATION = 300,
    BACKWARD = 301,
    BEFORE = 302,
    BEGIN_P = 303,
    BETWEEN = 304,
    BIGINT = 305,
    BINARY = 306,
    BIT = 307,
    BOOLEAN_P = 308,
    BOTH = 309,
    BY = 310,
    CACHE = 311,
    CALL = 312,
    CALLED = 313,
    CASCADE = 314,
    CASCADED = 315,
    CASE = 316,
    CAST = 317,
    CATALOG_P = 318,
    CHAIN = 319,
    CHAR_P = 320,
    CHARACTER = 321,
    CHARACTERISTICS = 322,
    CHECK = 323,
    CHECKPOINT = 324,
    CLASS = 325,
    CLOSE = 326,
    CLUSTER = 327,
    COALESCE = 328,
    COLLATE = 329,
    COLLATION = 330,
    COLUMN = 331,
    COLUMNS = 332,
    COMMENT = 333,
    COMMENTS = 334,
    COMMIT = 335,
    COMMITTED = 336,
    CONCURRENTLY = 337,
    CONFIGURATION = 338,
    CONFLICT = 339,
    CONNECTION = 340,
    CONSTRAINT = 341,
    CONSTRAINTS = 342,
    CONTENT_P = 343,
    CONTINUE_P = 344,
    CONVERSION_P = 345,
    COPY = 346,
    COST = 347,
    CREATE = 348,
    CROSS = 349,
    CSV = 350,
    CUBE = 351,
    CURRENT_P = 352,
    CURRENT_CATALOG = 353,
    CURRENT_DATE = 354,
    CURRENT_ROLE = 355,
    CURRENT_SCHEMA = 356,
    CURRENT_TIME = 357,
    CURRENT_TIMESTAMP = 358,
    CURRENT_USER = 359,
    CURSOR = 360,
    CYCLE = 361,
    DATA_P = 362,
    DATABASE = 363,
    DAY_P = 364,
    DEALLOCATE = 365,
    DEC = 366,
    DECIMAL_P = 367,
    DECLARE = 368,
    DEFAULT = 369,
    DEFAULTS = 370,
    DEFERRABLE = 371,
    DEFERRED = 372,
    DEFINER = 373,
    DELETE_P = 374,
    DELIMITER = 375,
    DELIMITERS = 376,
    DEPENDS = 377,
    DESC = 378,
    DETACH = 379,
    DICTIONARY = 380,
    DISABLE_P = 381,
    DISCARD = 382,
    DISTINCT = 383,
    DO = 384,
    DOCUMENT_P = 385,
    DOMAIN_P = 386,
    DOUBLE_P = 387,
    DROP = 388,
    EACH = 389,
    ELSE = 390,
    ENABLE_P = 391,
    ENCODING = 392,
    ENCRYPTED = 393,
    END_P = 394,
    ENUM_P = 395,
    ESCAPE = 396,
    EVENT = 397,
    EXCEPT = 398,
    EXCLUDE = 399,
    EXCLUDING = 400,
    EXCLUSIVE = 401,
    EXECUTE = 402,
    EXISTS = 403,
    EXPLAIN = 404,
    EXPRESSION = 405,
    EXTENSION = 406,
    EXTERNAL = 407,
    EXTRACT = 408,
    FALSE_P = 409,
    FAMILY = 410,
    FETCH = 411,
    FILTER = 412,
    FIRST_P = 413,
    FLOAT_P = 414,
    FOLLOWING = 415,
    FOR = 416,
    FORCE = 417,
    FOREIGN = 418,
    FORWARD = 419,
    FREEZE = 420,
    FROM = 421,
    FULL = 422,
    FUNCTION = 423,
    FUNCTIONS = 424,
    GENERATED = 425,
    GLOBAL = 426,
    GRANT = 427,
    GRANTED = 428,
    GREATEST = 429,
    GROUP_P = 430,
    GROUPING = 431,
    GROUPS = 432,
    HANDLER = 433,
    HAVING = 434,
    HEADER_P = 435,
    HOLD = 436,
    HOUR_P = 437,
    IDENTITY_P = 438,
    IF_P = 439,
    ILIKE = 440,
    IMMEDIATE = 441,
    IMMUTABLE = 442,
    IMPLICIT_P = 443,
    IMPORT_P = 444,
    IN_P = 445,
    INCLUDE = 446,
    INCLUDING = 447,
    INCREMENT = 448,
    INDEX = 449,
    INDEXES = 450,
    INHERIT = 451,
    INHERITS = 452,
    INITIALLY = 453,
    INLINE_P = 454,
    INNER_P = 455,
    INOUT = 456,
    INPUT_P = 457,
    INSENSITIVE = 458,
    INSERT = 459,
    INSTEAD = 460,
    INT_P = 461,
    INTEGER = 462,
    INTERSECT = 463,
    INTERVAL = 464,
    INTO = 465,
    INVOKER = 466,
    IS = 467,
    ISNULL = 468,
    ISOLATION = 469,
    JOIN = 470,
    KEY = 471,
    LABEL = 472,
    LANGUAGE = 473,
    LARGE_P = 474,
    LAST_P = 475,
    LATERAL_P = 476,
    LEADING = 477,
    LEAKPROOF = 478,
    LEAST = 479,
    LEFT = 480,
    LEVEL = 481,
    LIKE = 482,
    LIMIT = 483,
    LISTEN = 484,
    LOAD = 485,
    LOCAL = 486,
    LOCALTIME = 487,
    LOCALTIMESTAMP = 488,
    LOCATION = 489,
    LOCK_P = 490,
    LOCKED = 491,
    LOGGED = 492,
    MAPPING = 493,
    MATCH = 494,
    MATERIALIZED = 495,
    MAXVALUE = 496,
    METHOD = 497,
    MINUTE_P = 498,
    MINVALUE = 499,
    MODE = 500,
    MONTH_P = 501,
    MOVE = 502,
    NAME_P = 503,
    NAMES = 504,
    NATIONAL = 505,
    NATURAL = 506,
    NCHAR = 507,
    NEW = 508,
    NEXT = 509,
    NO = 510,
    NONE = 511,
    NOT = 512,
    NOTHING = 513,
    NOTIFY = 514,
    NOTNULL = 515,
    NOWAIT = 516,
    NULL_P = 517,
    NULLIF = 518,
    NULLS_P = 519,
    NUMERIC = 520,
    OBJECT_P = 521,
    OF = 522,
    OFF = 523,
    OFFSET = 524,
    OIDS = 525,
    OLD = 526,
    ON = 527,
    ONLY = 528,
    OPERATOR = 529,
    OPTION = 530,
    OPTIONS = 531,
    OR = 532,
    ORDER = 533,
    ORDINALITY = 534,
    OTHERS = 535,
    OUT_P = 536,
    OUTER_P = 537,
    OVER = 538,
    OVERLAPS = 539,
    OVERLAY = 540,
    OVERRIDING = 541,
    OWNED = 542,
    OWNER = 543,
    PARALLEL = 544,
    PARSER = 545,
    PARTIAL = 546,
    PARTITION = 547,
    PASSING = 548,
    PASSWORD = 549,
    PLACING = 550,
    PLANS = 551,
    POLICY = 552,
    POSITION = 553,
    PRECEDING = 554,
    PRECISION = 555,
    PRESERVE = 556,
    PREPARE = 557,
    PREPARED = 558,
    PRIMARY = 559,
    PRIOR = 560,
    PRIVILEGES = 561,
    PROCEDURAL = 562,
    PROCEDURE = 563,
    PROCEDURES = 564,
    PROGRAM = 565,
    PUBLICATION = 566,
    QUOTE = 567,
    RANGE = 568,
    READ = 569,
    REAL = 570,
    REASSIGN = 571,
    RECHECK = 572,
    RECURSIVE = 573,
    REF = 574,
    REFERENCES = 575,
    REFERENCING = 576,
    REFRESH = 577,
    REINDEX = 578,
    RELATIVE_P = 579,
    RELEASE = 580,
    RENAME = 581,
    REPEATABLE = 582,
    REPLACE = 583,
    REPLICA = 584,
    RESET = 585,
    RESTART = 586,
    RESTRICT = 587,
    RETURNING = 588,
    RETURNS = 589,
    REVOKE = 590,
    RIGHT = 591,
    ROLE = 592,
    ROLLBACK = 593,
    ROLLUP = 594,
    ROUTINE = 595,
    ROUTINES = 596,
    ROW = 597,
    ROWS = 598,
    RULE = 599,
    SAVEPOINT = 600,
    SCHEMA = 601,
    SCHEMAS = 602,
    SCROLL = 603,
    SEARCH = 604,
    SECOND_P = 605,
    SECURITY = 606,
    SELECT = 607,
    SEQUENCE = 608,
    SEQUENCES = 609,
    SERIALIZABLE = 610,
    SERVER = 611,
    SESSION = 612,
    SESSION_USER = 613,
    SET = 614,
    SETS = 615,
    SETOF = 616,
    SHARE = 617,
    SHOW = 618,
    SIMILAR = 619,
    SIMPLE = 620,
    SKIP = 621,
    SMALLINT = 622,
    SNAPSHOT = 623,
    SOME = 624,
    SQL_P = 625,
    STABLE = 626,
    STANDALONE_P = 627,
    START = 628,
    STATEMENT = 629,
    STATISTICS = 630,
    STDIN = 631,
    STDOUT = 632,
    STORAGE = 633,
    STORED = 634,
    STRICT_P = 635,
    STRIP_P = 636,
    SUBSCRIPTION = 637,
    SUBSTRING = 638,
    SUPPORT = 639,
    SYMMETRIC = 640,
    SYSID = 641,
    SYSTEM_P = 642,
    TABLE = 643,
    TABLES = 644,
    TABLESAMPLE = 645,
    TABLESPACE = 646,
    TEMP = 647,
    TEMPLATE = 648,
    TEMPORARY = 649,
    TEXT_P = 650,
    THEN = 651,
    TIES = 652,
    TIME = 653,
    TIMESTAMP = 654,
    TO = 655,
    TRAILING = 656,
    TRANSACTION = 657,
    TRANSFORM = 658,
    TREAT = 659,
    TRIGGER = 660,
    TRIM = 661,
    TRUE_P = 662,
    TRUNCATE = 663,
    TRUSTED = 664,
    TYPE_P = 665,
    TYPES_P = 666,
    UESCAPE = 667,
    UNBOUNDED = 668,
    UNCOMMITTED = 669,
    UNENCRYPTED = 670,
    UNION = 671,
    UNIQUE = 672,
    UNKNOWN = 673,
    UNLISTEN = 674,
    UNLOGGED = 675,
    UNTIL = 676,
    UPDATE = 677,
    USER = 678,
    USING = 679,
    VACUUM = 680,
    VALID = 681,
    VALIDATE = 682,
    VALIDATOR = 683,
    VALUE_P = 684,
    VALUES = 685,
    VARCHAR = 686,
    VARIADIC = 687,
    VARYING = 688,
    VERBOSE = 689,
    VERSION_P = 690,
    VIEW = 691,
    VIEWS = 692,
    VOLATILE = 693,
    WHEN = 694,
    WHERE = 695,
    WHITESPACE_P = 696,
    WINDOW = 697,
    WITH = 698,
    WITHIN = 699,
    WITHOUT = 700,
    WORK = 701,
    WRAPPER = 702,
    WRITE = 703,
    XML_P = 704,
    XMLATTRIBUTES = 705,
    XMLCONCAT = 706,
    XMLELEMENT = 707,
    XMLEXISTS = 708,
    XMLFOREST = 709,
    XMLNAMESPACES = 710,
    XMLPARSE = 711,
    XMLPI = 712,
    XMLROOT = 713,
    XMLSERIALIZE = 714,
    XMLTABLE = 715,
    YEAR_P = 716,
    YES_P = 717,
    ZONE = 718,
    NOT_LA = 719,
    NULLS_LA = 720,
    WITH_LA = 721,
    POSTFIXOP = 722,
    UMINUS = 723
  };
#endif

/* Value type.  */
#if ! defined YYSTYPE && ! defined YYSTYPE_IS_DECLARED
union YYSTYPE
{
#line 203 "gram.y"

	core_YYSTYPE		core_yystype;
	/* these fields must match core_YYSTYPE: */
	int					ival;
	char				*str;
	const char			*keyword;

	char				chr;
	bool				boolean;
	JoinType			jtype;
	DropBehavior		dbehavior;
	OnCommitAction		oncommit;
	List				*list;
	Node				*node;
	Value				*value;
	ObjectType			objtype;
	TypeName			*typnam;
	FunctionParameter   *fun_param;
	FunctionParameterMode fun_param_mode;
	ObjectWithArgs		*objwithargs;
	DefElem				*defelt;
	SortBy				*sortby;
	WindowDef			*windef;
	JoinExpr			*jexpr;
	IndexElem			*ielem;
	Alias				*alias;
	RangeVar			*range;
	IntoClause			*into;
	WithClause			*with;
	InferClause			*infer;
	OnConflictClause	*onconflict;
	A_Indices			*aind;
	ResTarget			*target;
	struct PrivTarget	*privtarget;
	AccessPriv			*accesspriv;
	struct ImportQual	*importqual;
	InsertStmt			*istmt;
	VariableSetStmt		*vsetstmt;
	PartitionElem		*partelem;
	PartitionSpec		*partspec;
	PartitionBoundSpec	*partboundspec;
	RoleSpec			*rolespec;

#line 570 "gram.h"

};
typedef union YYSTYPE YYSTYPE;
# define YYSTYPE_IS_TRIVIAL 1
# define YYSTYPE_IS_DECLARED 1
#endif

/* Location type.  */
#if ! defined YYLTYPE && ! defined YYLTYPE_IS_DECLARED
typedef struct YYLTYPE YYLTYPE;
struct YYLTYPE
{
  int first_line;
  int first_column;
  int last_line;
  int last_column;
};
# define YYLTYPE_IS_DECLARED 1
# define YYLTYPE_IS_TRIVIAL 1
#endif



int base_yyparse (core_yyscan_t yyscanner);

#endif /* !YY_BASE_YY_GRAM_H_INCLUDED  */
