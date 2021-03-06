/* Company :       Nequeo Pty Ltd, http://www.nequeo.com.au/
*  Copyright :     Copyright � Nequeo Pty Ltd 2014 http://www.nequeo.com.au/
*
*  File :          PriorityStrategy.h
*  Purpose :       PriorityStrategy class.
*
*/

/*
Permission is hereby granted, free of charge, to any person
obtaining a copy of this software and associated documentation
files (the "Software"), to deal in the Software without
restriction, including without limitation the rights to use,
copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the
Software is furnished to do so, subject to the following
conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
OTHER DEALINGS IN THE SOFTWARE.
*/

#pragma once

#ifndef _PRIORITYSTRATEGY_H
#define _PRIORITYSTRATEGY_H

#include "GlobalThreading.h"

#include "NotificationStrategy.h"
#include "SharedPtr.h"
#include <vector>

namespace Nequeo {
	namespace Threading {
		namespace Events
		{
			/// NotificationStrategy for PriorityEvent.
			///
			/// Delegates are kept in a std::vector<>, ordered
			/// by their priority.
			template <class TArgs, class TDelegate>
			class PriorityStrategy : public NotificationStrategy < TArgs, TDelegate >
			{
			public:
				typedef SharedPtr<TDelegate>         DelegatePtr;
				typedef std::vector<DelegatePtr>     Delegates;
				typedef typename Delegates::iterator Iterator;

			public:
				PriorityStrategy()
				{
				}

				PriorityStrategy(const PriorityStrategy& s) :
					_delegates(s._delegates)
				{
				}

				~PriorityStrategy()
				{
				}

				void notify(const void* sender, TArgs& arguments)
				{
					for (Iterator it = _delegates.begin(); it != _delegates.end(); ++it)
					{
						(*it)->notify(sender, arguments);
					}
				}

				void add(const TDelegate& delegate)
				{
					for (Iterator it = _delegates.begin(); it != _delegates.end(); ++it)
					{
						if ((*it)->priority() > delegate.priority())
						{
							_delegates.insert(it, DelegatePtr(static_cast<TDelegate*>(delegate.clone())));
							return;
						}
					}
					_delegates.push_back(DelegatePtr(static_cast<TDelegate*>(delegate.clone())));
				}

				void remove(const TDelegate& delegate)
				{
					for (Iterator it = _delegates.begin(); it != _delegates.end(); ++it)
					{
						if (delegate.equals(**it))
						{
							(*it)->disable();
							_delegates.erase(it);
							return;
						}
					}
				}

				PriorityStrategy& operator = (const PriorityStrategy& s)
				{
					if (this != &s)
					{
						_delegates = s._delegates;
					}
					return *this;
				}

				void clear()
				{
					for (Iterator it = _delegates.begin(); it != _delegates.end(); ++it)
					{
						(*it)->disable();
					}
					_delegates.clear();
				}

				bool empty() const
				{
					return _delegates.empty();
				}

			protected:
				Delegates _delegates;
			};
		}
	}
}
#endif
