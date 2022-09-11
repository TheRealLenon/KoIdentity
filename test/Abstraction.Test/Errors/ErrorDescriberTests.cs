// KoIdentity Copyright (C) 2022 Tekoding. All Rights Reserved.
// 
// Created: 2022.05.29
// 
// Authors: TheRealLenon
// 
// Licensed under the MIT License. See LICENSE.md in the project root for license
// information.
// 
// KoIdentity is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the MIT
// License for more details.

using FluentAssertions;
using NUnit.Framework;
using Tekoding.KoIdentity.Abstraction.Errors;

namespace Tekoding.KoIdentity.Abstraction.Test.Errors;

/// <summary>
/// Defines a set of unit tests for the <see cref="ErrorDescriber"/> describing <see cref="Error"/>s.
/// </summary>
public class ErrorDescriberTests
{
    /// <summary>
    /// Checking the <see cref="ErrorDescriber"/> to return an <b>ObjectNullFailure</b> with the correct code and
    /// description.
    /// </summary>
    [Test]
    public void ErrorDescriber_ValidateObjectNullFailure()
    {
        var error = ErrorDescriber.ObjectNullFailure();

        error.Code.Should().NotBeNullOrWhiteSpace().And.Be(nameof(ErrorDescriber.ObjectNullFailure));
        error.Description.Should().NotBeNullOrWhiteSpace().And.Contain("memory");
    }
}