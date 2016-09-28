/* Company :       Nequeo Pty Ltd, http://www.nequeo.com.au/
*  Copyright :     Copyright � Nequeo Pty Ltd 2016 http://www.nequeo.com.au/
*
*  File :          AwsAccount.cpp
*  Purpose :       AWS account class.
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

#include "stdafx.h"

#include "AwsAccount.h"

using namespace Nequeo::Aws::Storage;

static const char* ACCOUNT_CLIENT_TAG = "NequeoAccountClient";

///	<summary>
///	AWS account provider.
///	</summary>
/// <param name="credentials">The credentials object for AWS services.</param>
/// <param name="clientConfiguration">Configuration for accessing Amazon web services.</param>
AwsAccount::AwsAccount(const AWS::Auth::AWSCredentials& credentials, const AWS::Client::ClientConfiguration& clientConfiguration) : 
	_disposed(false), _initialized(false), _credentials(credentials), _clientConfiguration(clientConfiguration)
{
}

///	<summary>
///	AWS account provider.
///	</summary>
AwsAccount::~AwsAccount()
{
	if (!_disposed)
	{
		_disposed = true;
		
		// Clenup Aws.
		AWS::ShutdownAPI(_options);
		_initialized = false;
	}
}

///	<summary>
///	Initialize the AWS SDK, this must be call after setting options.
///	</summary>
void AwsAccount::Initialize()
{
	if (!_initialized)
	{
		// Initialize SDK wide state for the SDK.
		AWS::InitAPI(_options);
		_initialized = true;
	}
}

///	<summary>
///	Get the region.
///	</summary>
///	<return>The region.</return>
std::string AwsAccount::GetRegion()
{
	return std::string(_clientConfiguration.region.c_str());
}

///	<summary>
///	Set the region.
///	</summary>
/// <param name="region">The region.</param>
void AwsAccount::SetRegion(const std::string& region)
{
	_clientConfiguration.region = AWS::String(region.c_str());
}

///	<summary>
///	Set the defualt client configuration executor.
///	</summary>
void AwsAccount::SetDefaultExecutor()
{
	// Create a new executor.
	_executor = AWS::MakeShared<AWS::Utils::Threading::DefaultExecutor>(ACCOUNT_CLIENT_TAG);
	_clientConfiguration.executor = _executor;
}

///	<summary>
///	Set the pooled thread client configuration executor.
///	</summary>
/// <param name="poolSize">The thread pool size.</param>
void AwsAccount::SetPooledExecutor(size_t poolSize)
{
	// Create a new executor.
	_executor = AWS::MakeShared<AWS::Utils::Threading::PooledThreadExecutor>(ACCOUNT_CLIENT_TAG, poolSize);
	_clientConfiguration.executor = _executor;
}