/**************************************************************************
ALGLIB 3.10.0 (source code generated 2015-08-19)
Copyright (c) Sergey Bochkanov (ALGLIB project).

>>> SOURCE LICENSE >>>
This program is free software; you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation (www.fsf.org); either version 2 of the 
License, or (at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

A copy of the GNU General Public License is available at
http://www.fsf.org/licensing/licenses
>>> END OF LICENSE >>>
**************************************************************************/

#ifndef _testhqrndunit_h
#define _testhqrndunit_h

#include "aenv.h"
#include "ialglib.h"
#include "apserv.h"
#include "hqrnd.h"


/*$ Declarations $*/


/*$ Body $*/


ae_bool testhqrnd(ae_bool silent, ae_state *_state);
ae_bool _pexec_testhqrnd(ae_bool silent, ae_state *_state);


/*************************************************************************
Function for test HQRNDContinuous function
*************************************************************************/
ae_bool hqrndcontinuoustest(ae_bool silent, ae_state *_state);


/*************************************************************************
Function for test HQRNDContinuous function
*************************************************************************/
ae_bool hqrnddiscretetest(ae_bool silent, ae_state *_state);


/*$ End $*/
#endif

